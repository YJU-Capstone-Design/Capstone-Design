using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyUnit : UnitBase
{
    [Header("# Unit Setting")]
    public Scanner scanner;
    public UnitData unitData;
    Vector3 moveVec; // 이동 방향
    LayerMask attackLayer;
    [SerializeField] Vector3 attackRayPos; // attackRay 위치 = 현재 위치 + attackRayPos
    [SerializeField] Vector2 attackRaySize;
    GameObject hpBar; // 체력바
    [SerializeField] Transform[] allChildren;

    [Header("# Unit Activity")]
    Collider2D col;
    public RaycastHit2D[] attackTargets; // 스캔 결과 배열
    [SerializeField] Transform nearestAttackTarget; // 가장 가까운 목표
    [SerializeField] Transform[] multipleAttackTargets; // 다수 공격 목표
    MonsterCharacterAnimation anim;
    Coroutine smash; // 코루틴 값을 저장하기 위한 변수
    Coroutine arrow;

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<MonsterCharacterAnimation>();

        allChildren = GetComponentsInChildren<Transform>();

        attackLayer = LayerMask.GetMask("PlayerUnit", "Wall");
    }

    void OnEnable()
    {
        CardManger.Instance.enemys.Add(gameObject);
        StateSetting();
    }

    private void Start()
    {
        // 초기 데이터 저장
        initialHealth = unitData.Health;
        initialSpeed = unitData.Speed;
        initialPower = unitData.Power;
        initialAttackTime = unitData.AttackTime;
    }

    void Update()
    {
        
        if (unitState != UnitState.Die)
        {
            // 체력 실시간 적용
            HpBar hpBarLogic = hpBar.GetComponent<HpBar>();
            hpBarLogic.nowHp = health;
            hpBarLogic.hpBarDir = moveVec;

            if (health <= 0)
            {
                health = 0;
                StartCoroutine(Die());
            }
            else
            {
                AttackRay();
            }
        }

        // 모든 Sprite 자식 오브젝트 Order Layer 조정
        foreach (Transform child in allChildren)
        {
            SpriteRenderer bodySprite = child.GetComponent<SpriteRenderer>();

            // SpriteRenderer 가 있을 경우에는 본체의 y 축 값의 소수점을 제외한 값을 Order Layer 에 적용
            if(bodySprite != null)
            {
                float yPos = (transform.position.y - 4) * 10; // 음수/양수 처리를 위해 -4, 넓게 분배하기 위해 *10
                int orderLayer = Mathf.FloorToInt(yPos); // 소수점 제외
                bodySprite.sortingOrder = Mathf.Abs(orderLayer); // 절대값으로 변경 후 적용
            }
        }
    }

    void OnDisable()
    {
        transform.position = new Vector3(10, 0, 0); // 위치 초기화 (안해주면 다시 소환되는 순간  Unit 의 Ray 영역 안에 있으면 Ray 에 잠시 인식됨.)
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
        moveVec = Vector3.left;
        transform.GetChild(0).rotation = Quaternion.identity; // 애니메이션 각도 초기화를 위한 로직
        scanner.unitType = unitID / 10000;

        // 체력바
        hpBar = PoolManager.Instance.Get(1, 3);
        HpBar hpBarLogic = hpBar.GetComponent<HpBar>();
        hpBarLogic.owner = this.gameObject.transform;
        hpBarLogic.nowHp = health;
        hpBarLogic.maxHp = health;
    }

    // 가까운 적을 찾는 Scanner 함수
    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // 위치 차이 = 타겟 위치 - 나의 위치  ->  (방향) 
            moveVec = scanner.nearestTarget.position - transform.position;

            if (transform.position.y != moveVec.y) { moveVec.x *= 0.5f; moveVec.y *= 2f; } // y 축 먼저 빠르게 이동
            else { moveVec.x *= 1f; moveVec.y *= 1f; } // 정상화
        }
        else
        {
            // 인식된 적이 없을 시에는 왼쪽으로 전진
            moveVec = Vector3.left;
        }

        // 목표 지점으로 이동
        transform.position += moveVec.normalized * speed * Time.deltaTime;
        unitState = UnitState.Move;

        // 가는 방향에 따라 Sprite 방향 변경
        SpriteDir(Vector3.zero, moveVec);

        // 애니메이션
        anim.Walk();
    }

    // 실제 공격 범위 Ray 함수
    void AttackRay()
    {
        attackTargets = Physics2D.BoxCastAll(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y * (moveVec.y > 0 ? 2 : 1), attackRayPos.z), attackRaySize, 0, Vector2.zero, 0, attackLayer);
        nearestAttackTarget = scanner.GetNearestAttack(attackTargets); // 단일 공격
        multipleAttackTargets = scanner.GetAttackTargets(attackTargets, 5); // 다수 공격


        if (nearestAttackTarget != null)
        {
            // 적(유닛, 벽)이 인식되면 attackTime 증가 및 공격 함수 실행
            attackTime += Time.deltaTime;

            if (!nearestAttackTarget.CompareTag("Wall"))
            {
                // 적 상태 변경
                if ((unitID % 10000) / 1000 == 2) // 탱커 -> 다수 공격
                {
                    if (multipleAttackTargets == null) return;
                    foreach (Transform enemy in multipleAttackTargets)
                    {
                        if (enemy == null || !enemy.CompareTag("Wall")) continue;
                        UnitBase enemyState = enemy.gameObject.GetComponent<UnitBase>();
                        enemyState.unitState = UnitState.Fight;
                    }
                }
                else
                {
                    if (nearestAttackTarget == null) return;
                    UnitBase enemyState = nearestAttackTarget.gameObject.GetComponent<UnitBase>();
                    enemyState.unitState = UnitState.Fight;
                }
            }

            // 공격
            if (attackTime >= unitData.AttackTime)
            {
                attackTime = 0;

                // 유닛 별로 각각의 공격 함수 실행
                if (gameObject.CompareTag("Archer"))
                {
                    arrow = StartCoroutine(Arrow());
                }
                else
                {
                    smash = StartCoroutine(Attack());
                }
            }

            // 적의 위치에 따라 Sprite 방향 변경 (Attary Ray 영역이 큰 Unit 변수 제거)
            SpriteDir(transform.position, nearestAttackTarget.transform.position);
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
        if (nearestAttackTarget == null)
        {
            if (smash != null) { StopCoroutine(smash); smash = null; }
        }

        // 유닛 종류 별 애니메이션
        switch (gameObject.name)
        {
            case string name when name.Contains("Zombie") || name.Contains("Cyclope") || name.Contains("Orc"):
                anim.Smash();
                break;
            case string name when name.Contains("Skeleton"):
                anim.Stab();
                break;
            default:
                anim.Attack();
                break;
        }

        if (nearestAttackTarget.gameObject.CompareTag("Wall"))
        {
            yield return new WaitForSeconds(anim.GetTime());

            BattleManager.Instance.HpDamage(power);

        }
        else
        {
            // Cyclope 랑 Orc 만 첫번째 공격이 도중에 끊겨서 일단 문제를 찾기 전까지 분류해서 시간 나눔.
            if (gameObject.name.Contains("Cyclope") || gameObject.name.Contains("Orc")) { yield return new WaitForSeconds(anim.GetTime() + 0.3f); }
            else { yield return new WaitForSeconds(anim.GetTime()); }

            if ((unitID % 10000) / 1000 == 2) // 탱커 -> 다수 공격
            {
                foreach(Transform enemy in multipleAttackTargets)
                {
                    SetEnemyState(enemy);
                }
            } 
            else // 단일 공격
            {
                SetEnemyState(nearestAttackTarget);
            }
        }

        // 애니메이션
        anim.Idle();
    }

    void SetEnemyState(Transform target)
    {
        if (target == null)
            return;

        PlayerUnit enemyLogic = target.gameObject.GetComponent<PlayerUnit>();

        enemyLogic.health -= power;
    }

    // 화살 공격 함수
    IEnumerator Arrow()
    {
        if (nearestAttackTarget == null){
            if (arrow != null) { StopCoroutine(arrow); arrow = null; }
        }

        // 애니메이션
        anim.Bow();

        yield return new WaitForSeconds(0.3f);

        if (!nearestAttackTarget.gameObject.CompareTag("Wall"))
        {
            // 맞고 있는 적 유닛 상태 변경
            PlayerUnit enemyLogic = nearestAttackTarget.gameObject.GetComponent<PlayerUnit>();
        }

        // 화살 가져오기
        GameObject arrowObj = PoolManager.Instance.Get(3, 0, transform.position + new Vector3(0, 0.5f, 0));
        Arrow arrawLogic = arrowObj.GetComponent<Arrow>();
        arrawLogic.unitType = unitID / 10000;
        arrawLogic.arrowPower = power;

        // 화살 목표 오브젝트 설정
        arrawLogic.target = nearestAttackTarget.gameObject;
        arrawLogic.playerUnit = this.gameObject;

        yield return new WaitForSeconds(anim.GetTime());

        // 애니메이션
        anim.Idle();
    }



    IEnumerator Die()
    {
        unitState = UnitState.Die;
        moveVec = Vector2.zero;
        col.enabled = false;

        PlayerUnit enemyLogic = nearestAttackTarget.GetComponent<PlayerUnit>();
        enemyLogic.unitState = UnitState.Move;

        // 작동중인 다른 Coroutine 함수 중지
        if (smash != null) { StopCoroutine(smash); smash = null; }
        if (arrow != null) { StopCoroutine(arrow); arrow = null; }

        speed = 0;
        attackTime = 0;

        CardManger.Instance.enemys.Remove(gameObject);

        // 애니메이션
        anim.Die();


        yield return new WaitForSeconds(anim.GetTime());

        StateSetting();
        hpBar.SetActive(false);
        gameObject.SetActive(false);
    }

}
