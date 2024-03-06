using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    Scanner scanner;
    Rigidbody2D rigid;

    [Header("# Unit Setting")]
    public LayerMask attackLayer;
    Vector2 moveDir; //  ����
    Vector2 disVec; // �Ÿ�
    Vector2 nextVec; // ������ ������ ��ġ�� ��
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
        rigid = GetComponent<Rigidbody2D>();
        scanner = GetComponentInChildren<Scanner>();

        StateSetting();
    }

    void OnEnable()
    {
        StateSetting();
        unitState = UnitState.Move;
        moveDir = Vector3.right;

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
        attackTime = 0f;
    }

    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // ��ġ ���� = Ÿ�� ��ġ - ���� ��ġ
            disVec = (Vector2)scanner.nearestTarget.position - rigid.position;
            Enemy enemyLogic = scanner.nearestTarget.GetComponent<Enemy>();

            // �̵�
            nextVec = disVec.normalized * speed * Time.deltaTime;
            transform.position += (Vector3)nextVec;
            unitState = UnitState.Move;

            // ���� ���⿡ ���� Sprite ���� ����
            if (disVec.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                moveDir = Vector2.right;
            }
            else if (disVec.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                moveDir = Vector2.left;
            }

        }
        else
        {
            if(startMoveFinish)
            {
                moveDir = Vector2.zero;
                transform.localScale = new Vector3(1f, 1f, 1f);
                unitState = UnitState.Idle;
            }
        }
    }

    void AttackRay()
    {
        Collider2D attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.5f, 0.5f), 0, attackLayer);

        if (attackTarget != null)
        {
            Enemy targetLogic = attackTarget.gameObject.GetComponent<Enemy>();

            if (unitState != UnitState.Attack)
            {
                unitState = UnitState.Fight;
            }

            startMoveFinish = true;

            // ���� �νĵǸ� attackTime ���� �� ���� �Լ� ����
            attackTime += Time.deltaTime;

            if (attackTime >= unitData.AttackTime && unitState != UnitState.Attack)
            {
                attackTime = 0;
                StartCoroutine("Attack");
            }

            gameObject.layer = 8;
        }
        else
        {
            gameObject.layer = 6;
            
            // AttackRay �� �νĵǴ� ������Ʈ�� ���� ���, �ٽ� ��ĵ ����
            Scanner();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.5f, 0.5f));
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

    IEnumerator Attack()
    {
        Debug.Log("Attack");
        unitState = UnitState.Attack;

        yield return new WaitForSeconds(3.5f);

        unitState = UnitState.Fight;
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
        moveDir = Vector2.zero;
        disVec = Vector2.zero;
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
