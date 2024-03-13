using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    Scanner scanner;

    [Header("# Unit Setting")]
    LayerMask targetLayer;
    Vector3 moveVec; // �Ÿ�
    public Vector3 attackRayPos; // attackRay ��ġ = ���� ��ġ + attackRayPos
    public Vector2 attackRaySize;
    bool startMoveFinish = false;
    public UnitData unitData;

    [Header("# Unit Activity")]
    new Collider2D collider;
    Collider2D attackTarget;

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
        collider = GetComponent<Collider2D>();

        targetLayer = scanner.targetLayer;
    }

    void OnEnable()
    {
        StateSetting();

        StartCoroutine(
            lerpCoroutine(GameManager.Instance.unitSpawnPoint[0].position, GameManager.Instance.point, speed));
    }

    void Update()
    {
        AttackRay();
        // Animation();

        if(health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    void StateSetting()
    {
        // ��ġ��
        unitID = unitData.UnitID;
        health = unitData.Health;
        speed = unitData.Speed;
        power = unitData.Power;
        attackTime = unitData.AttackTime;

        // ������
        collider.enabled = true;
        unitState = UnitState.Move;
        moveVec = Vector3.right;
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
        attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize, 0, targetLayer);

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
        Gizmos.DrawWireCube(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize);
    }

    void Attack()
    {
        EnemyUnit enemyLogic = attackTarget.gameObject.GetComponent<EnemyUnit>();

        enemyLogic.health -= power;
    }

    IEnumerator Die()
    {
        collider.enabled = false;
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
