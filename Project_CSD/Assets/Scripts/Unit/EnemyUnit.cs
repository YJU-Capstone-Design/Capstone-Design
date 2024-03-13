using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : UnitBase
{
    [Header("# Unit Setting")]
    Scanner scanner;
    public UnitData unitData;
    LayerMask targetLayer;
    Vector3 moveVec; // 이동 방향
    public LayerMask attackLayer;
    public Vector3 attackRayPos; // attackRay 위치 = 현재 위치 + attackRayPos
    public Vector2 attackRaySize;

    [Header("# Unit Activity")]
    Collider2D col;
    Collider2D attackTarget;

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();
        col = GetComponent<Collider2D>();

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
        col.enabled = true;
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
        attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y * (moveVec.y > 0 ? -1 : 1), attackRayPos.z), attackRaySize, 0, attackLayer);

        if (attackTarget != null)
        {
            unitState = UnitState.Fight;

            // 적(유닛, 벽)이 인식되면 attackTime 증가 및 공격 함수 실행
            attackTime += Time.deltaTime;

            if(attackTime >= unitData.AttackTime)
            {
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
        if (attackTarget.gameObject.CompareTag("Wall"))
        {
            /*MainWall wallLogic = attackTarget.gameObject.GetComponent<MainWall>();

            wallLogic.health -= power;*/
            BattleManager.Instance.HpDamage(power);
            attackTime = 0;

            //Debug.Log(wallLogic.health);
        } else
        {
            PlayerUnit enemyLogic = attackTarget.gameObject.GetComponent<PlayerUnit>();

            enemyLogic.health -= power;
            attackTime = 0;
        }
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

}
