using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyUnit : UnitBase
{
    [Header("# Unit Setting")]
    Scanner scanner;
    public UnitData unitData;
    LayerMask targetLayer;
    Vector3 moveVec; // 이동 방향
    public LayerMask attackLayer;
    public Vector3 attackRayPos; // attackRay 위치 = 현재 위치 + attackRayPos
    public Vector2 attackRaySize;

    [Header("# Unit Activity")]
    Collider2D col;
    public RaycastHit2D[] attackTargets; // 스캔 결과 배열
    [SerializeField] public Transform nearestAttackTarget; // 가장 가까운 목표
    MonsterCharacterAnimation anim;

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<MonsterCharacterAnimation>();

        targetLayer = scanner.targetLayer;

        StateSetting();
    }

    void OnEnable()
    {
        StateSetting();
    }

    void Update()
    {
        if (unitState != UnitState.Die)
        {
            if (health <= 0)
            {
                health = 0;
                StartCoroutine(Die());
                // Debug.Log("Die");
                //gameObject.SetActive(false);
            }
            else
            {
                AttackRay();
            }
        }
    }

    // 기본 설정 초기화 함수
    void StateSetting()
    {
        // 수치값
        unitID = unitData.UnitID;
        health = unitData.Health;
        speed = unitData.Speed;
        power = unitData.Power;
        attackTime = unitData.AttackTime;

        // 설정값
        col.enabled = true;
        unitState = UnitState.Move;
        unitActivity = UnitActivity.Normal;
        moveVec = Vector3.left;
        transform.GetChild(0).rotation = Quaternion.identity; // 애니메이션 각도 초기화를 위한 로직
        scanner.unitType = unitID / 10000;
    }

    // 가까운 적을 찾는 Scanner 함수
    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // 위치 차이 = 타겟 위치 - 나의 위치  ->  (방향) 
            moveVec = scanner.nearestTarget.position - transform.position;
        }
        else
        {
            // 인식된 적이 없을 시에는 왼쪽으로 전진
            moveVec = Vector2.left;
        }

        // 목표 지점으로 이동
        transform.position += moveVec.normalized * speed * Time.deltaTime;
        unitState = UnitState.Move;

        // 가는 방향에 따라 Sprite 방향 변경
        if (moveVec.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (moveVec.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // 애니메이션
        anim.Walk();
    }

    // 실제 공격 범위 Ray 함수
    void AttackRay()
    {
        attackTargets = Physics2D.BoxCastAll(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y * (moveVec.y > 0 ? -1 : 1), attackRayPos.z), attackRaySize, 0, Vector2.zero, 0, attackLayer);
        nearestAttackTarget = scanner.GetNearestAttack(attackTargets);

        if (nearestAttackTarget != null)
        {
            unitState = UnitState.Fight;

            // 적(유닛, 벽)이 인식되면 attackTime 증가 및 공격 함수 실행
            attackTime += Time.deltaTime;

            if (attackTime >= unitData.AttackTime)
            {
                attackTime = 0;

                // 유닛 별로 각각의 공격 함수 실행
                if (gameObject.CompareTag("Archer"))
                {
                    StartCoroutine(Arrow());
                }
                else
                {
                    StartCoroutine(Attack());
                }
            }

            // 적의 위치에 따라 Sprite 방향 변경 (Attary Ray 영역이 큰 Unit 변수 제거)
            if (nearestAttackTarget.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (nearestAttackTarget.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        else
        {
            // Attack Ray 에 인식된 적이 없을 경우에 Scanner 활성화
            Scanner();

            // 다음에 attackRay 에 적 인식시, 바로 공격 가능하게 attackTime 초기화
            attackTime = unitData.AttackTime - 0.2f;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y * (moveVec.y > 0 ? 2 : 1), attackRayPos.z), attackRaySize);
    }

    // 일반 근접 공격 함수
    IEnumerator Attack()
    {
        if (nearestAttackTarget == null) StopCoroutine(Attack());

        // 애니메이션
        anim.Smash();

        if (nearestAttackTarget.gameObject.CompareTag("Wall"))
        {
            yield return new WaitForSeconds(anim.GetTime());

            BattleManager.Instance.curHealth -= power;

        } else
        {
            PlayerUnit enemyLogic = nearestAttackTarget.gameObject.GetComponent<PlayerUnit>();
            enemyLogic.unitActivity = UnitActivity.Hit;

            yield return new WaitForSeconds(anim.GetTime() + 0.5f);

            enemyLogic.health -= power;

            // 애니메이션
            anim.Idle();

            yield return new WaitForSeconds(anim.GetTime() + 1f);

            // 맞은 직후 다시 상대의 UnitActivity 는 normal 상태로 변경
            enemyLogic.unitActivity = UnitActivity.Normal;
        }
    }

    // 화살 공격 함수
    IEnumerator Arrow()
    {
        if (nearestAttackTarget == null) StopCoroutine(Arrow());

        // 애니메이션
        anim.Bow();

        yield return new WaitForSeconds(0.3f);

        if (!nearestAttackTarget.gameObject.CompareTag("Wall"))
        {
            // 맞고 있는 적 유닛 상태 변경
            PlayerUnit enemyLogic = nearestAttackTarget.gameObject.GetComponent<PlayerUnit>();
            enemyLogic.unitActivity = UnitBase.UnitActivity.Hit;
        }

        // 화살 가져오기
        GameObject arrow = PoolManager.Instance.Get(3, 0, transform.position + new Vector3(0, 0.5f, 0));
        Arrow arrawLogic = arrow.GetComponent<Arrow>();
        arrawLogic.unitType = unitID / 10000;
        arrawLogic.arrowPower = power;

        // 화살 목표 오브젝트 설정
        arrawLogic.target = nearestAttackTarget.gameObject;
        arrawLogic.playerUnit = this.gameObject;

        yield return new WaitForSeconds(anim.GetTime() + 0.2f);

        // 애니메이션
        anim.Idle();
    }



    public IEnumerator Die()
    {
        unitState = UnitState.Die;
        moveVec = Vector2.zero;
        col.enabled = false;
        unitActivity = UnitActivity.Normal;

        speed = 0;
        attackTime = 0;

        // 애니메이션
        anim.Die();

        yield return new WaitForSeconds(anim.GetTime());

        StateSetting();
        transform.position = new Vector3(10, 0, 0); // 위치 초기화 (안해주면 다시 소환되는 순간  Unit 의 Ray 영역 안에 있으면 Ray 에 잠시 인식됨.)
        gameObject.SetActive(false);
    }

}
