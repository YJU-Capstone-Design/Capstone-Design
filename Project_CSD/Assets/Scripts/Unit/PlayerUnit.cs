using System.Collections;
using System.Collections.Generic;
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
    Collider2D attackTarget;
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
        AttackRay();
        // Animation();

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

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
        moveVec = Vector3.right;
        firstPos = GameManager.Instance.point;
    }

    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // 위치 차이(방향) = 타겟 위치 - 나의 위치
            moveVec = scanner.nearestTarget.position - transform.position;
            // 이동
            transform.position += moveVec.normalized * speed * Time.deltaTime;

            // 이동
            //StartCoroutine(
                //lerpCoroutine(transform.position, scanner.nearestTarget.position, speed));


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

    void AttackRay()
    {
        attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize, 0, targetLayer);

        if (attackTarget != null)
        {
            EnemyUnit targetLogic = attackTarget.gameObject.GetComponent<EnemyUnit>();
            unitState = UnitState.Fight;

            startMoveFinish = true;

            // 적이 인식되면 attackTime 증가 및 공격 함수 실행
            attackTime += Time.deltaTime;

            if (attackTime >= unitData.AttackTime)
            {
                attackTime = 0;
                Attack();
            }
        }
        else
        {
            // AttackRay 에 인식되는 오브젝트가 없는 경우, 다시 스캔 시작
            Scanner();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize);
    }

    void Attack()
    {
        EnemyUnit enemyLogic = attackTarget.gameObject.GetComponent<EnemyUnit>();

        enemyLogic.health -= power;
    }

    IEnumerator Die()
    {
        col.enabled = false;
        unitState = UnitState.Die;
        attackTime = 0;
        moveVec = Vector2.zero;
        speed = 0;

        yield return new WaitForSeconds(1f);

        Debug.Log("Die");
        gameObject.SetActive(false);
    }

    IEnumerator lerpCoroutine(Vector3 current, Vector3 target, float speed)
    {

        float distance = Vector3.Distance(current, target); // 거리(길이) 구하기
        float time = distance / speed; // 거리(길이) 에 따라 이동하는 시간 설정

        float elapsedTime = 0.0f;

        // 경계선 범위 벗어나지 않게 설정
        if (target.y >= 2) { target.y = 2; }
        else if (target.y <= -2) { target.y = -2; }
        else if (target.x >= 6) { target.x = 6; }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            moveVec.y = 0;
        }
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
