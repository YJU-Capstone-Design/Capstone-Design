using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tanker : PlayerUnit
{
    bool isDefending = false;

    protected override void Scanner()
    {
        if(isDefending)
        {
            // 애니메이션
            StartAnimation("Defense_end", false, 1f);

            isDefending = false;
        }

        base.Scanner();
    }

    protected override void AttackRay()
    {
        // BoxCastAll(시작 위치, 크기, 회전, 방향, 길이, 대상 레이어) : 사각형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        attackTargets = Physics2D.BoxCastAll(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize, 0, Vector2.zero, 0, targetLayer);

        nearestAttackTarget = scanner.GetNearestAttack(attackTargets); // 단일 공격
        multipleAttackTargets = scanner.GetAttackTargets(attackTargets, 5); // 다수 공격

        if (nearestAttackTarget != null)
        {
            if (!startMoveFinish)
            {
                StopCoroutine(lerp);
                startMoveFinish = true;
            }

            // 적이 인식되면 attackTime 증가 및 공격 함수 실행
            attackTime += Time.deltaTime;

            // 적 상태 변경
            if (multipleAttackTargets == null)
                return;

            foreach (Transform enemy in multipleAttackTargets)
            {
                if (enemy == null) return;
                EnemyUnit enemyLogic = enemy.gameObject.GetComponent<EnemyUnit>();
                enemyLogic.unitState = UnitState.Fight; // 적 상태 변경

                // 적 공격 목표를 자신으로 변경
                // 다수 공격 적에, 공격 목표에 자신이 포함되어 있지 않을 떼,
                if ((enemyLogic.unitID % 10000) / 1000 == 2 && !enemyLogic.multipleAttackTargets.Contains(this.gameObject.transform))
                {
                    if(enemyLogic.nearestAttackTarget != null)
                    {
                        enemyLogic.multipleAttackTargets[0] = this.gameObject.transform;
                    }
                }
                else if ((enemyLogic.unitID % 10000) / 1000 != 2) // 단일 공격 적일 때
                {
                    enemyLogic.nearestAttackTarget = this.gameObject.transform;
                }

                enemyLogic.scanner.nearestTarget = this.gameObject.transform;
            }

            // 방어
            if (attackTime >= unitData.AttackTime && !isDefending)
            {
                attackTime = 0;

                smash = StartCoroutine(Attack());
            }


            // 적의 위치에 따라 Sprite 방향 변경 (Attary Ray 영역이 큰 Unit 변수 제거 용도)
            SpriteDir(nearestAttackTarget.transform.position, transform.position);
        }
        else
        {
            // AttackRay 에 인식되는 오브젝트가 없는 경우, 다시 스캔 시작
            Scanner();

            // 다음에 attackRay 에 적 인식시, 바로 공격 가능하게 attackTime 초기화
            attackTime = unitData.AttackTime - 0.2f;
        }
    }


    protected override IEnumerator Attack()
    {
        if (nearestAttackTarget == null)
        {
            if (smash != null) { StopCoroutine(smash); smash = null; }
        }

        // 애니메이션
        StartAnimation("Defense_start", false, 1f);

        yield return new WaitForSeconds(0.6f); // 애니메이션 시간

        if ((unitID % 10000) / 1000 == 2) // 탱커 -> 다수 공격
        {
            foreach (Transform enemy in multipleAttackTargets)
            {
                SetEnemyState(enemy);
            }
        }

        yield return new WaitForSeconds(0.4f); // 애니메이션 시간

        StartAnimation("Defense_ing", false, 1f);

        isDefending = true;
    }
}
