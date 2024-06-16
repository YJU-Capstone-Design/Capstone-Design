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
            // �ִϸ��̼�
            StartAnimation("Defense_end", false, 1f);

            isDefending = false;
        }

        base.Scanner();
    }

    protected override void AttackRay()
    {
        // BoxCastAll(���� ��ġ, ũ��, ȸ��, ����, ����, ��� ���̾�) : �簢���� ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ�
        attackTargets = Physics2D.BoxCastAll(transform.position + new Vector3(attackRayPos.x * Mathf.Sign(moveVec.x), attackRayPos.y, attackRayPos.z), attackRaySize, 0, Vector2.zero, 0, targetLayer);

        nearestAttackTarget = scanner.GetNearestAttack(attackTargets); // ���� ����
        multipleAttackTargets = scanner.GetAttackTargets(attackTargets, 5); // �ټ� ����

        if (nearestAttackTarget != null)
        {
            if (!startMoveFinish)
            {
                StopCoroutine(lerp);
                startMoveFinish = true;
            }

            // ���� �νĵǸ� attackTime ���� �� ���� �Լ� ����
            attackTime += Time.deltaTime;

            // �� ���� ����
            if (multipleAttackTargets == null)
                return;

            foreach (Transform enemy in multipleAttackTargets)
            {
                if (enemy == null) return;
                EnemyUnit enemyLogic = enemy.gameObject.GetComponent<EnemyUnit>();
                enemyLogic.unitState = UnitState.Fight; // �� ���� ����

                // �� ���� ��ǥ�� �ڽ����� ����
                // �ټ� ���� ����, ���� ��ǥ�� �ڽ��� ���ԵǾ� ���� ���� ��,
                if ((enemyLogic.unitID % 10000) / 1000 == 2 && !enemyLogic.multipleAttackTargets.Contains(this.gameObject.transform))
                {
                    if(enemyLogic.nearestAttackTarget != null)
                    {
                        enemyLogic.multipleAttackTargets[0] = this.gameObject.transform;
                    }
                }
                else if ((enemyLogic.unitID % 10000) / 1000 != 2) // ���� ���� ���� ��
                {
                    enemyLogic.nearestAttackTarget = this.gameObject.transform;
                }

                enemyLogic.scanner.nearestTarget = this.gameObject.transform;
            }

            // ���
            if (attackTime >= unitData.AttackTime && !isDefending)
            {
                attackTime = 0;

                smash = StartCoroutine(Attack());
            }


            // ���� ��ġ�� ���� Sprite ���� ���� (Attary Ray ������ ū Unit ���� ���� �뵵)
            SpriteDir(nearestAttackTarget.transform.position, transform.position);
        }
        else
        {
            // AttackRay �� �νĵǴ� ������Ʈ�� ���� ���, �ٽ� ��ĵ ����
            Scanner();

            // ������ attackRay �� �� �νĽ�, �ٷ� ���� �����ϰ� attackTime �ʱ�ȭ
            attackTime = unitData.AttackTime - 0.2f;
        }
    }


    protected override IEnumerator Attack()
    {
        if (nearestAttackTarget == null)
        {
            if (smash != null) { StopCoroutine(smash); smash = null; }
        }

        // �ִϸ��̼�
        StartAnimation("Defense_start", false, 1f);

        yield return new WaitForSeconds(0.6f); // �ִϸ��̼� �ð�

        if ((unitID % 10000) / 1000 == 2) // ��Ŀ -> �ټ� ����
        {
            foreach (Transform enemy in multipleAttackTargets)
            {
                SetEnemyState(enemy);
            }
        }

        yield return new WaitForSeconds(0.4f); // �ִϸ��̼� �ð�

        StartAnimation("Defense_ing", false, 1f);

        isDefending = true;
    }
}
