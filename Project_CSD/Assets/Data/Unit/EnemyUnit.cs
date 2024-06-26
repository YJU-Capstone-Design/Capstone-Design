using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] float attackRayUpPos; // 유닛이 위를 향할 때 사용되는 y 포지션 값
    [SerializeField] float attackRayDownPos; // 유닛이 아래를 향할 때 사용되는 y 포지션 값 -> 기본값 (Enemy 는 아래를 향할 때 중심과 크게 차이가 안나게 설정)
    [SerializeField] Vector3 attackRayPos; // attackRay 위치 = 현재 위치 + attackRayPos
    [SerializeField] Vector2 attackRaySize;
    GameObject hpBar; // 체력바
    [SerializeField] Transform[] allChildren;

    [Header("# Unit Activity")]
    Collider2D col;
    public RaycastHit2D[] attackTargets; // 스캔 결과 배열
    [SerializeField] public Transform nearestAttackTarget; // 가장 가까운 목표
    [SerializeField] public Transform[] multipleAttackTargets; // 다수 공격 목표
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
        BattleData.Instance.enemys.Add(gameObject);
        StateSetting(BattleManager.Instance.wave);
        MakeHpBar();
        Debug.Log("Enable");
    }

    private void Start()
    {
        // 초기 데이터 저장
        initialHealth = unitData.Health;
        initialMoveSpeed = unitData.MoveSpeed;
        initialPower = unitData.Power;
        initialAttackTime = unitData.AttackTime;
    }

    void Update()
    {
        if (unitState != UnitState.Die)
        {
            if(hpBar != null)
            {
                // 체력 실시간 적용
                HpBar hpBarLogic = hpBar.GetComponent<HpBar>();
                hpBarLogic.nowHp = health;
            }

            if (health <= 0)
            {
                health = 0;
                StartCoroutine(Die());
            }
            else
            {
                AttackRay();

                // 아군 유닛 최대 전진 범위 때문에 설정
                if (transform.position.x <= 28) { col.enabled = true; } else {  col.enabled = false; }
            }
        }

        // 모든 Sprite 자식 오브젝트 Order Layer 조정
        foreach (Transform child in allChildren)
        {
            SpriteRenderer bodySprite = child.GetComponent<SpriteRenderer>();

            // SpriteRenderer 가 있을 경우에는 본체의 y 축 값의 소수점을 제외한 값을 Order Layer 에 적용
            if(bodySprite != null)
            {
                float yPos = transform.position.y * 100 - 400; // 넓게 분배하기 위해 * 100 음수/양수 처리를 위해 -400;
                int orderLayer = Mathf.FloorToInt(yPos); // 소수점 제외
                if(bodySprite.gameObject.name.Contains("Shadow"))
                {
                    bodySprite.sortingOrder = Mathf.Abs(orderLayer) - 1; // 그림자는 -1
                }
                else
                {
                    bodySprite.sortingOrder = Mathf.Abs(orderLayer); // 절대값으로 변경 후 적용
                }

                // 체력바 OrderLayer
                if(hpBar != null)
                {
                    HpBar hpBarLogic = hpBar.GetComponent<HpBar>();
                    hpBarLogic.realHpSprite.sortingOrder = Mathf.Abs(orderLayer) - 1;
                    hpBarLogic.hpFrameSprite.sortingOrder = Mathf.Abs(orderLayer);
                }
            }
        }
    }

    void OnDisable()
    {
        transform.position = new Vector3(32, 0, 0); // 위치 초기화 (안해주면 다시 소환되는 순간  Unit 의 Ray 영역 안에 있으면 Ray 에 잠시 인식됨.)
    }

    // 기본 설정 초기화 함수
    void StateSetting(int wave)
    {
        // 수치값
        unitID = unitData.UnitID;
        health = unitData.Health + (unitData.Health / 10) * wave;
        moveSpeed = unitData.MoveSpeed;
        power = unitData.Power + (unitData.Power / 10) * wave;
        attackTime = unitData.AttackTime;

        // 설정값
        transform.parent.position = Vector3.zero;
        transform.position = Vector3.zero;
        unitState = UnitState.Move;
        moveVec = Vector3.left;
        transform.GetChild(0).rotation = Quaternion.identity; // 애니메이션 각도 초기화를 위한 로직
        scanner.unitType = unitID / 10000;
        multipleAttackTargets = new Transform[5];
    }

    // 체력바 생성
    void MakeHpBar()
    {
        hpBar = PoolManager.Instance.Get(1, 4);
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
        transform.position += moveVec.normalized * moveSpeed * Time.deltaTime;
        unitState = UnitState.Move;

        // 가는 방향에 따라 Sprite 방향 변경
        SpriteDir(Vector3.zero, moveVec);

        // 애니메이션
        if(gameObject.name.Contains("Bat"))
        {
            anim.Idle();
        }
        else if(gameObject.name.Contains("Beholder") || gameObject.name.Contains("Crow"))
        {
            anim.Fly();
        }
        else
        {
            anim.Walk();
        }
    }

    // 실제 공격 범위 Ray 함수
    void AttackRay()
    {
        if (transform.position.x <= 28)
        {
            attackTargets = Physics2D.BoxCastAll(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), (moveVec.y > 0 ? attackRayUpPos : attackRayDownPos), attackRayPos.z), attackRaySize, 0, Vector2.zero, 0, attackLayer);
            nearestAttackTarget = scanner.GetNearestAttack(attackTargets); // 단일 공격
            multipleAttackTargets = scanner.GetAttackTargets(attackTargets, 5); // 다수 공격
        }


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
        Gizmos.DrawWireCube(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), (moveVec.y > 0 ? attackRayUpPos : attackRayDownPos), attackRayPos.z), attackRaySize);
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
            case string name when name.Contains("Zombie") || name.Contains("Cyclope") || name.Contains("Orc") || name.Contains("Goblin") || name.Contains("Ghost") || name.Contains("Demon"):
                anim.Smash();
                break;
            case string name when name.Contains("Skeleton"):
                anim.Stab();
                break;
            default:
                anim.Attack();
                break;
        }

        // 특정 유닛들만 첫번째 공격이 도중에 끊겨서 일단 문제를 찾기 전까지 분류해서 시간 나눔.
        string[] namesToCheck = { "Cyclope", "Orc", "Rat", "Goblin", "Demon", "Worm"};

        if (namesToCheck.Any(name => gameObject.name.Contains(name)))
        {
            yield return new WaitForSeconds(anim.GetTime() + 0.3f);
        }
        else
        {
            yield return new WaitForSeconds(anim.GetTime());
        }

        if (nearestAttackTarget != null)
        {
            if (nearestAttackTarget.gameObject.CompareTag("Wall"))
            {
                BattleManager.Instance.HpDamage(power);

            }
            else
            {
                if ((unitID % 10000) / 1000 == 2) // 탱커 -> 다수 공격
                {
                    foreach (Transform enemy in multipleAttackTargets)
                    {
                        SetEnemyState(enemy);
                    }
                }
                else // 단일 공격
                {
                    SetEnemyState(nearestAttackTarget);
                }
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
        Debug.Log(nearestAttackTarget.name);
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
        hpBar.SetActive(false);
        Debug.Log("Die");

        if (nearestAttackTarget != null)
        {
            PlayerUnit enemyLogic = nearestAttackTarget.GetComponent<PlayerUnit>();
            enemyLogic.unitState = UnitState.Move;

            nearestAttackTarget = null;
        }

        // 작동중인 다른 Coroutine 함수 중지
        if (smash != null) { StopCoroutine(smash); smash = null; }
        if (arrow != null) { StopCoroutine(arrow); arrow = null; }

        moveSpeed = 0;
        attackTime = 0;

        BattleData.Instance.enemys.Remove(gameObject);

        // 애니메이션
        anim.Die();

        yield return new WaitForSeconds(anim.GetTime());

        StateSetting(0);

        // 부모 오브젝트를 종료
        transform.parent.gameObject.SetActive(false);
        //transform.gameObject.SetActive(false);
    }
}
