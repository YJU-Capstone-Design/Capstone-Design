using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnitBase;

public class PlayerUnit : UnitBase
{
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
    Vector3 firstPos;

    [Header("# Spine")]
    //스파인 애니메이션을 위한 것
    //public SkeletonAnimation skeletonAnimation;
    //public AnimationReferenceAsset[] AnimClip;

    //현재 애니메이션 처리가 무엇인지에 대한 변수

    //private AnimState _AnimState;
    //현재 어떤 애니메이션이 재생되고 있는지에 대한 변수
    private string CurrentAnimation;

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();
        col = GetComponent<Collider2D>();

        targetLayer = scanner.targetLayer;

        StateSetting();
    }

    void OnEnable()
    {
        StateSetting();

        // 클릭 지점으로 이동
        StartCoroutine(
            lerpCoroutine(GameManager.Instance.unitSpawnPoint[0].position, GameManager.Instance.point, speed));
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

    // 기본 설정 초기화 함수
    void StateSetting()
    {
        // 수치값
        unitID = unitData.UnitID;
        health = unitData.Health;
        speed = unitData.Speed;
        power = unitData.Power;
        attackTime = unitData.AttackTime - 0.5f;

        // 설정값
        col.enabled = true;
        unitState = UnitState.Move;
        unitActivity = UnitActivity.Normal;
        moveVec = Vector3.right;
        firstPos = GameManager.Instance.point;
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


            // 가는 방향에 따라 Sprite 방향 변경
            if (moveVec.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (moveVec.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }

        }
        else
        {
            if (startMoveFinish)
            {
                // 유닛의 처음 위치로 귀환
                StartCoroutine(
                    lerpCoroutine(transform.position, firstPos, speed));

                moveVec = Vector2.zero;
                transform.localScale = new Vector3(1f, 1f, 1f);
                unitState = UnitState.Idle;
            }
        }
    }

    // 가까운 공격 목표를 찾는 Ray 함수 (공격)

    void AttackRay()
    {
        // BoxCastAll(시작 위치, 크기, 회전, 방향, 길이, 대상 레이어) : 사각형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        attackTargets = Physics2D.BoxCastAll(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize, 0, Vector2.zero, 0, targetLayer);

        nearestAttackTarget = scanner.GetNearestAttack(attackTargets);

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
            // AttackRay 에 인식되는 오브젝트가 없는 경우, 다시 스캔 시작
            Scanner();

            // 다음에 attackRay 에 적 인식시, 바로 공격 가능하게 attackTime 초기화
            attackTime = unitData.AttackTime - 0.5f;
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
        yield return null; // 나중에 아마도 애니메이션 시간에 맞춰서 변경 필요

        if (nearestAttackTarget == null) StopCoroutine(Attack());

        EnemyUnit enemyLogic = nearestAttackTarget.gameObject.GetComponent<EnemyUnit>();
        enemyLogic.unitActivity = UnitActivity.Hit;

        yield return new WaitForSeconds(3f);
        enemyLogic.health -= power;
        yield return new WaitForSeconds(1f);

        // 맞은 직후 다시 상대의 UnitActivity 는 normal 상태로 변경
        enemyLogic.unitActivity = UnitActivity.Normal;

    }

    // 화살 공격 함수
    IEnumerator Arrow()
    {
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

        yield return new WaitForSeconds(1f);

        transform.position = GameManager.Instance.unitSpawnPoint[0].position; // 위치 초기화 (안해주면 다시 소환되는 순간  Unit 의 Ray 영역 안에 있으면 Ray 에 잠시 인식됨.)
        gameObject.SetActive(false);
    }

    // 맨 처음 시작 이동 lerpCoroutine
    IEnumerator lerpCoroutine(Vector3 current, Vector3 target, float speed)
    {

        float distance = Vector3.Distance(current, target); // 거리(길이) 구하기
        float time = distance / speed; // 거리(길이) 에 따라 이동하는 시간 설정

        float elapsedTime = 0.0f;

        // 경계선 범위 벗어나지 않게 설정
        if (target.y >= 2) { target.y = 2; }
        else if (target.y <= -2) { target.y = -2; }
        else if (target.x >= 6) { target.x = 6; }
        else if (target.x <= -7) { target.x = -7; }

        this.transform.position = current;
        while (elapsedTime < time && !scanner.nearestTarget)
        {
            elapsedTime += Time.deltaTime;

            this.transform.position = Vector3.Lerp(current, target, elapsedTime / time);

            yield return null;

            unitState = UnitState.Move;

        }

        startMoveFinish = true;

        yield return null;
    }


    //void Animation()
    //{
    //    int animIndex = 0;

    //    switch(unitState)
    //    {
    //        case UnitState.Idle:
    //            _AnimState = AnimState.Idle;
    //            animIndex = 0;
    //            break;
    //        case UnitState.Move:
    //            _AnimState = AnimState.Run;
    //            animIndex = 1;
    //            break;
    //        case UnitState.Fight:
    //            Debug.Log("Fight Animation");
    //            _AnimState = AnimState.Idle;
    //            animIndex = 0;
    //            break;
    //        case UnitState.Attack:
    //            Debug.Log("Attack Animation");
    //            break;
    //        case UnitState.Damaged:
    //            break;
    //        case UnitState.Die:
    //            break;

    //    }

    //    //애니메이션 적용
    //    _AsyncAnimation(AnimClip[animIndex], true, 1f);
    //}

    //private void _AsyncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    //{
    //    //동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 X
    //    if (animClip.name.Equals(CurrentAnimation))
    //    {
    //        return;
    //    }

    //    //해당 애니메이션으로 변경한다.
    //    skeletonAnimation.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;
    //    skeletonAnimation.loop = loop;
    //    skeletonAnimation.timeScale = timeScale;

    //    //현재 재생되고 있는 애니메이션 값을 변경
    //    CurrentAnimation = animClip.name;
    //}

    //private void SetCurrentAnimation(AnimState _state)
    //{
    //    switch (_state)
    //    {
    //        case AnimState.Idle:
    //            _AsyncAnimation(AnimClip[(int)AnimState.Idle], true, 1f);
    //            break;
    //        case AnimState.Run:
    //            _AsyncAnimation(AnimClip[(int)AnimState.Run], true, 1f);
    //            break;
    //    }

    //    //짧게 작성한다 요렇게..
    //    //_AsyncAnimation(AnimClip[(int)AnimState], true, 1f);
    //}
}
