using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnitBase;
using Spine.Unity;
using static UnityEngine.GraphicsBuffer;

public class PlayerUnit : UnitBase
{
    [Header("# Unit Effect")]
    public List<GameObject> buffEffect = new List<GameObject>();


    [Header("# Unit Setting")]
    Scanner scanner;
    public UnitData unitData;
    bool startMoveFinish = false;
    LayerMask targetLayer;
    Vector3 moveVec; // 거리
    public Vector3 attackRayPos; // attackRay 위치 = 현재 위치 + attackRayPos
    public Vector2 attackRaySize;

    [Header("# Unit Activity")]
    Collider2D col;
    RaycastHit2D[] attackTargets; // 스캔 결과 배열
    [SerializeField] Transform nearestAttackTarget; // 가장 가까운 목표
    [SerializeField] Transform[] multipleAttackTargets; // 다수 공격 목표
    Vector3 firstPos;
    Coroutine smash;
    Coroutine arrow;
    Coroutine lerp;

    [Header("# Spine")]
    SkeletonAnimation skeletonAnimation;
    string CurrentAnimation; //현재 어떤 애니메이션이 재생되고 있는지에 대한 변수

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();
        col = GetComponent<Collider2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        targetLayer = scanner.targetLayer;

        StateSetting();
    }

    void OnEnable()
    {
        StateSetting();

        CardManger.Instance.units.Add(gameObject);
        // 클릭 지점으로 이동
        lerp = StartCoroutine(lerpCoroutine(BattleManager.Instance.unitSpawnPoint[0].position, BattleManager.Instance.point, speed));
    }

    void Update()
    {
        // Animation();
        if (unitState != UnitState.Die)
        {
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
            else
            {
                AttackRay();
            }
        }
    }

    void OnDisable()
    {
        transform.position = new Vector3(-10, 0, 0); // 위치 초기화 (안해주면 다시 소환되는 순간  Unit 의 Ray 영역 안에 있으면 Ray 에 잠시 인식됨.)
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
        moveVec = Vector3.right;
        firstPos = BattleManager.Instance.point;
        scanner.unitType = unitID / 10000;
        nearestAttackTarget = null;
    }

    // 가까운 적을 찾는 Scanner 함수 (이동)
    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // 위치 차이(방향) = 타겟 위치 - 나의 위치
            moveVec = scanner.nearestTarget.position - transform.position;
            if (moveVec.y > 0) { moveVec.y += 0.5f; }
            else if (moveVec.y < 0) { moveVec.y -= 0.5f; }

            // 이동
            transform.position += moveVec.normalized * speed * Time.deltaTime;

            // 애니메이션
            StartAnimation("walk", true, 1.2f);

            // 가는 방향에 따라 Sprite 방향 변경
            SpriteDir(moveVec, Vector3.zero);
        }
        else
        {
            if (startMoveFinish)
            {
                // 유닛의 처음 위치로 귀환
                StartCoroutine(lerpCoroutine(transform.position, firstPos, speed));

                if (transform.position == firstPos) {

                    moveVec = Vector3.zero;
                    unitState = UnitState.Idle;
                    transform.localScale = new Vector3(1f, 1f, 1f);

                    // 애니메이션
                    StartAnimation("idle", true, 1.5f);
                }
            }
        }
    }

    // 가까운 공격 목표를 찾는 Ray 함수 (공격)

    void AttackRay()
    {
        // BoxCastAll(시작 위치, 크기, 회전, 방향, 길이, 대상 레이어) : 사각형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        attackTargets = Physics2D.BoxCastAll(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize, 0, Vector2.zero, 0, targetLayer);

        nearestAttackTarget = scanner.GetNearestAttack(attackTargets); // 단일 공격
        multipleAttackTargets = scanner.GetAttackTargets(attackTargets, 5); // 다수 공격

        if (nearestAttackTarget != null)
        {
            unitState = UnitState.Fight;
            startMoveFinish = true;

            // 적이 인식되면 attackTime 증가 및 공격 함수 실행
            attackTime += Time.deltaTime;

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

            // 적의 위치에 따라 Sprite 방향 변경 (Attary Ray 영역이 큰 Unit 변수 제거 용도)
            SpriteDir(nearestAttackTarget.transform.position, transform.position);
        }
        else
        {
            // AttackRay 에 인식되는 오브젝트가 없는 경우, 다시 스캔 시작
            Scanner();

            // 다음에 attackRay 에 적 인식시, 바로 공격 가능하게 attackTime 초기화
            attackTime = unitData.AttackTime - 0.2f;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize);
    }

    // 일반 근접 공격 함수
    IEnumerator Attack()
    {
        if (nearestAttackTarget == null) StopCoroutine(smash);

        // 애니메이션
        StartAnimation("attack melee", false, 1f);

        if ((unitID % 10000) / 1000 == 2) // 탱커 -> 다수 공격
        {
            foreach (Transform enemy in multipleAttackTargets)
            {
                EnemyUnit enemyLogic = enemy.gameObject.GetComponent<EnemyUnit>();
                enemyLogic.unitActivity = UnitActivity.Hit;
            }
        }
        else
        {
            EnemyUnit enemyLogic = nearestAttackTarget.gameObject.GetComponent<EnemyUnit>();
            enemyLogic.unitActivity = UnitActivity.Hit;
        }

        yield return new WaitForSeconds(0.1f); // 애니메이션 시간

        if ((unitID % 10000) / 1000 == 2) // 탱커 -> 다수 공격
        {
            foreach (Transform enemy in multipleAttackTargets)
            {
                Hit(enemy);
            }
        }
        else // 단일 공격
        {
            Hit(nearestAttackTarget);
        }

        // 애니메이션
        StartAnimation("idle", true, 1f);
    }

    void Hit(Transform target)
    {
        EnemyUnit enemyLogic = target.gameObject.GetComponent<EnemyUnit>();

        enemyLogic.health -= power;

        // 맞은 직후 다시 상대의 UnitActivity 는 normal 상태로 변경
        enemyLogic.unitActivity = UnitActivity.Normal;
    }

    // 화살 공격 함수
    IEnumerator Arrow()
    {
        // 애니메이션
        StartAnimation("attack range", false, 1f);

        yield return null;

        if (nearestAttackTarget == null) StopCoroutine(Arrow());

        // 맞고 있는 적 유닛 상태 변경
        EnemyUnit enemyLogic = nearestAttackTarget.gameObject.GetComponent<EnemyUnit>();
        enemyLogic.unitActivity = UnitBase.UnitActivity.Hit;

        // 화살 가져오기
        GameObject arrow = PoolManager.Instance.Get(3, 0, transform.position + new Vector3(0, 0.5f, 0));
        Arrow arrowLogic = arrow.GetComponent<Arrow>();
        arrowLogic.unitType = unitID / 10000;
        arrowLogic.arrowPower = power;

        // 화살 목표 오브젝트 설정
        arrowLogic.target = nearestAttackTarget.gameObject;
        arrowLogic.playerUnit = this.gameObject;
    }

    IEnumerator Die()
    {
        unitState = UnitState.Die;
        moveVec = Vector2.zero;
        col.enabled = false;
        unitActivity = UnitActivity.Normal;

        speed = 0;
        attackTime = 0;

        CardManger.Instance.units.Remove(gameObject);

        // 진행중인 코루틴 함수 모두 중지
        if (smash != null) { StopCoroutine(smash); smash = null; }
        if (arrow != null) { StopCoroutine(arrow); arrow = null; }

        // 애니메이션
        // 아직 없음

        // 작동중인 다른 Coroutine 함수 중지
        StopCoroutine(Arrow());
        StopCoroutine(Attack());

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    // 맨 처음 시작 이동 lerpCoroutine
    IEnumerator lerpCoroutine(Vector3 current, Vector3 target, float speed)
    {

        float distance = Vector3.Distance(current, target); // 거리(길이) 구하기
        float time = distance / speed; // 거리(길이) 에 따라 이동하는 시간 설정

        float elapsedTime = 0.0f;

        this.transform.position = current;

        while (elapsedTime < time && !scanner.nearestTarget && current != target)
        {
            elapsedTime += Time.deltaTime;

            this.transform.position = Vector3.Lerp(current, target, elapsedTime / time);

            yield return null;

            unitState = UnitState.Move;

            // 가는 방향에 따라 Sprite 방향 변경
            SpriteDir(target, current);

            // 애니메이션
            StartAnimation("walk", true, 1.2f);
        }

        startMoveFinish = true;

        yield return null;
    }

    // 스파인 애니메이션 함수
    private void StartAnimation(string animName, bool loop, float timeScale)
    {
        //동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 X
        if (animName.Equals(CurrentAnimation))
        {
            return;
        }

        //해당 애니메이션으로 변경한다.
        skeletonAnimation.state.SetAnimation(0, animName, loop).TimeScale = timeScale;
        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        //현재 재생되고 있는 애니메이션 값을 변경
        CurrentAnimation = animName;
    }

    public void buff()
    {
        buffEffect[0].SetActive(true);
    }
}
