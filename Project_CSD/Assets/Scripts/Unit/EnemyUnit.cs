using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : UnitBase
{
    Scanner scanner;

    [Header("# Unit Setting")]
    LayerMask targetLayer;
    Vector3 moveVec; // 이동 방향
    public Vector3 attackRayPos; // attackRay 위치 = 현재 위치 + attackRayPos
    public Vector2 attackRaySize;
    public UnitData unitData;

    [Header("# Unit Activity")]
    new Collider2D collider;
    Collider2D attackTarget;

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();
        collider = GetComponent<Collider2D>();

        targetLayer = scanner.targetLayer;
    }

    void OnEnable()
    {
        StateSetting();
    }

    void Update()
    {
        AttackRay();

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
        collider.enabled = true;
        unitState = UnitState.Move;
        moveVec = Vector3.left;
    }

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
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveVec.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void AttackRay()
    {
        attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y * (moveVec.y > 0 ? -1 : 1), attackRayPos.z), attackRaySize, 0, targetLayer, 0, targetLayer);

        if (attackTarget != null)
        {
            PlayerUnit targetLogic = attackTarget.gameObject.GetComponent<PlayerUnit>();

            unitState = UnitState.Fight;

            // 적이 인식되면 attackTime 증가 및 공격 함수 실행
            attackTime += Time.deltaTime;

            if (attackTime >= unitData.AttackTime && targetLogic.unitState != UnitState.Die)
            {
                attackTime = 0;
                Attack();
            }
        }
        else
        {
            // Attack Ray 에 인식된 적이 없을 경우에 Scanner 활성화
            Scanner();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y * (moveVec.y > 0 ? -1 : 1), attackRayPos.z), attackRaySize);
    }

    void Attack()
    {
        PlayerUnit enemyLogic = attackTarget.gameObject.GetComponent<PlayerUnit>();

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

}
