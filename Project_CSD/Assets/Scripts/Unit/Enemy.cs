using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitBase
{
    Scanner scanner;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer renderer;

    public LayerMask attackLayer;
    Vector2 moveDir; //  ����
    Vector2 disVec; // �Ÿ�
    Vector2 nextVec; // ������ ������ ��ġ�� ��

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        scanner = GetComponentInChildren<Scanner>();

        unitState = UnitState.Move;
        moveDir = Vector3.left;
    }

    void Update()
    {
        AttackRay();
    }

    void Scanner()
    {
        if (scanner.nearestTarget)
        {
            // ��ġ ���� = Ÿ�� ��ġ - ���� ��ġ
            disVec = (Vector2)scanner.nearestTarget.position - rigid.position;

            // ���� ���⿡ ���� Sprite ���� ����
            if (disVec.x > 0)
            {
                renderer.flipX = true;
                moveDir = Vector2.right;
            }
            else if (disVec.x < 0)
            {
                renderer.flipX = false;
                moveDir = Vector2.left;
            }
        }
        else
        {
            disVec = Vector2.left;
            renderer.flipX = false;
        }

        // �̵�
        nextVec = disVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // ���� �ӵ��� MovePosition �̵��� ������ ���� �ʵ��� �ӵ� ����
        unitState = UnitState.Move;

        anim.SetInteger("AnimState", 2);
    }

    void AttackRay()
    {
        Collider2D attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.6f, 0.5f), 0, attackLayer);

        if (attackTarget != null)
        {
            PlayerUnit targetLogic = attackTarget.gameObject.GetComponent<PlayerUnit>();

            unitState = UnitState.Fight;
            anim.SetInteger("AnimState", 0);

            gameObject.layer = 9;
        }
        else
        {
            gameObject.layer = 7;

            // AttackRay �� �νĵǴ� ������Ʈ�� ���� ���, �ٽ� ��ĵ ����
            Scanner();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(moveDir.x * 0.45f, 0.3f, 0), new Vector2(0.6f, 0.5f));
    }

}
