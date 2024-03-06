using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    Scanner scanner;

    [Header("# Unit Setting")]
    LayerMask targetLayer;
    Vector3 moveVec; // �Ÿ�
    bool startMoveFinish = false;
    public UnitData unitData;

    [Header("# Spine")]
    //������ �ִϸ��̼��� ���� ��
    //public SkeletonAnimation skeletonAnimation;
    //public AnimationReferenceAsset[] AnimClip;

    //���� �ִϸ��̼� ó���� ���������� ���� ����
    //private AnimState _AnimState;

    //���� � �ִϸ��̼��� ����ǰ� �ִ����� ���� ����
    private string CurrentAnimation;

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();

        targetLayer = scanner.targetLayer;
    }

    void OnEnable()
    {
        StateSetting();
        unitState = UnitState.Move;
        moveVec = Vector3.right;

        StartCoroutine(
            lerpCoroutine(GameManager.Instance.unitSpawnPoint[0].position, GameManager.Instance.point, speed));
    }

    void OnDisable()
    {
        Die();
    }

    void Update()
    {
        AttackRay();
        // Animation();
    }

    void StateSetting()
    {
        unitID = unitData.UnitID;
        health = unitData.Health;
        speed = unitData.Speed;
        power = unitData.Power;
        attackTime = unitData.AttackTime;
    }

    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // ��ġ ���� = Ÿ�� ��ġ - ���� ��ġ
            moveVec = scanner.nearestTarget.position - transform.position;

            // �̵�
            transform.position += moveVec.normalized * speed * Time.deltaTime;
            unitState = UnitState.Move;

            // ���� ���⿡ ���� Sprite ���� ����
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
            if(startMoveFinish)
            {
                moveVec = Vector2.zero;
                transform.localScale = new Vector3(1f, 1f, 1f);
                unitState = UnitState.Idle;
            }
        }
    }

    void AttackRay()
    {
        Collider2D attackTarget = Physics2D.OverlapBox(transform.position + new Vector3((moveVec.x > 0 ? 0.5f : -0.5f), 0, 0), new Vector2(0.5f, 1f), 0, targetLayer);

        if (attackTarget != null)
        {
            EnemyUnit targetLogic = attackTarget.gameObject.GetComponent<EnemyUnit>();
            unitState = UnitState.Fight;

            startMoveFinish = true;

            // ���� �νĵǸ� attackTime ���� �� ���� �Լ� ����
            attackTime += Time.deltaTime;

            if (attackTime >= unitData.AttackTime)
            {
                attackTime = 0;
                Attack();
            }
        }
        else
        {      
            // AttackRay �� �νĵǴ� ������Ʈ�� ���� ���, �ٽ� ��ĵ ����
            Scanner();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3((moveVec.x > 0 ? 0.5f : -0.5f), 0, 0), new Vector2(0.5f, 1f));
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

    //    //�ִϸ��̼� ����
    //    _AsyncAnimation(AnimClip[animIndex], true, 1f);
    //}

    void Attack()
    {
        Debug.Log("Attack");
    }

    void Damaged()
    {
        if(health == 0)
        {
            Die();
        }
    }

    void Die()
    {
        unitState = UnitState.Die;
        attackTime = 0;
        startMoveFinish = false;
        moveVec = Vector2.zero;
    }

    IEnumerator lerpCoroutine(Vector3 current, Vector3 target, float speed)
    {
        float distance = Vector3.Distance(current, target); // �Ÿ�(����) ���ϱ�
        float time = distance / speed; // �Ÿ�(����) �� ���� �̵��ϴ� �ð� ����

        float elapsedTime = 0.0f;

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

    //private void _AsyncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    //{
    //    //������ �ִϸ��̼��� ����Ϸ��� �Ѵٸ� �Ʒ� �ڵ� ���� ���� X
    //    if (animClip.name.Equals(CurrentAnimation))
    //    {
    //        return;
    //    }

    //    //�ش� �ִϸ��̼����� �����Ѵ�.
    //    skeletonAnimation.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;
    //    skeletonAnimation.loop = loop;
    //    skeletonAnimation.timeScale = timeScale;

    //    //���� ����ǰ� �ִ� �ִϸ��̼� ���� ����
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

    //    //ª�� �ۼ��Ѵ� �䷸��..
    //    //_AsyncAnimation(AnimClip[(int)AnimState], true, 1f);
    //}
}
