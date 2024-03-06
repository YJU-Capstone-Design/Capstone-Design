using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyUnit : UnitBase
{
    Scanner scanner;

    public LayerMask targetLayer;
    Vector3 moveVec; // 이동 방향
    public UnitData unitData;

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();

        unitState = UnitState.Move;
        moveVec = Vector3.left;
    }

    void OnEnable()
    {
        StateSetting();
    }

    void Update()
    {
        AttackRay();
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
        Collider2D attackTarget = Physics2D.OverlapBox(transform.position + new Vector3((moveVec.x > 0 ? -0.5f : -0.5f), 0f, 0), new Vector2(0.5f, 1f), 0, targetLayer);

        if (attackTarget != null)
        {
            PlayerUnit targetLogic = attackTarget.gameObject.GetComponent<PlayerUnit>();

            unitState = UnitState.Fight;
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
        Gizmos.DrawWireCube(transform.position + new Vector3((moveVec.x > 0 ? -0.5f : -0.5f), 0, 0), new Vector2(0.5f, 1f));
    }

}
