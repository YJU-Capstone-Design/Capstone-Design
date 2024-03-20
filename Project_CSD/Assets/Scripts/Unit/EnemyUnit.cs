using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    Collider2D attackTarget; // 공격 목표
    MonsterCharacterAnimation anim;

    void Awake()
    {
        scanner = GetComponentInChildren<Scanner>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<MonsterCharacterAnimation>();

        targetLayer = scanner.targetLayer;
    }

    void OnEnable()
    {
        StateSetting();
    }

    void Update()
    {
        if (unitState != UnitState.Die)
        {
            if (health <= 0)
            {
                health = 0;
                StartCoroutine(Die());
                // Debug.Log("Die");
                //gameObject.SetActive(false);
            }
            else
            {
                AttackRay();
            }
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
        unitActivity = UnitActivity.Normal;
        moveVec = Vector3.left;
        transform.GetChild(0).rotation = Quaternion.identity;
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
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (moveVec.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // 애니메이션
        anim.Walk();
    }

    void AttackRay()
    {
        attackTarget = Physics2D.OverlapBox(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y * (moveVec.y > 0 ? -1 : 1), attackRayPos.z), attackRaySize, 0, attackLayer);
        
        if (attackTarget != null)
        {
            unitState = UnitState.Fight;

            // 적(유닛, 벽)이 인식되면 attackTime 증가 및 공격 함수 실행
            attackTime += Time.deltaTime;

            if (attackTime >= unitData.AttackTime)
            {
                attackTime = 0;
                StartCoroutine(Attack());
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
        Gizmos.DrawWireCube(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y * (moveVec.y > 0 ? 2 : 1), attackRayPos.z), attackRaySize);
    }

    IEnumerator Attack()
    {
        // 애니메이션
        anim.Smash();

        if (attackTarget.gameObject.CompareTag("Wall"))
        {
            MainWall wallLogic = attackTarget.gameObject.GetComponent<MainWall>();

            yield return new WaitForSeconds(anim.GetTime());

            wallLogic.health -= power;

        } else
        {
            PlayerUnit enemyLogic = attackTarget.gameObject.GetComponent<PlayerUnit>();
            enemyLogic.unitActivity = UnitActivity.Hit;

            yield return new WaitForSeconds(anim.GetTime());

            enemyLogic.health -= power;

            // 맞은 직후 다시 상대의 UnitActivity 는 normal 상태로 변경
            enemyLogic.unitActivity = UnitActivity.Normal;
        }
    }

    public IEnumerator Die()
    {
        unitState = UnitState.Die;
        moveVec = Vector2.zero;
        col.enabled = false;
        unitActivity = UnitActivity.Normal;

        speed = 0;
        attackTime = 0;

        // 애니메이션
        anim.Die();

        yield return new WaitForSeconds(anim.GetTime());

        StateSetting();
        gameObject.SetActive(false);
    }

}
