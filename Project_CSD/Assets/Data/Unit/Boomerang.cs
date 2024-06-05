using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : PlayerUnit
{
    protected override IEnumerator Attack()
    {
        if (nearestAttackTarget == null)
        {
            if (smash != null) { StopCoroutine(smash); smash = null; }
        }

        // �ִϸ��̼�
        StartAnimation("Attack", false, 1f);

        yield return new WaitForSeconds(0.4f); // �ִϸ��̼� �ð�

        // �� 2 �� ����
        foreach (Transform enemy in multipleAttackTargets)
        {
            SetEnemyState(enemy);
        }

        yield return new WaitForSeconds(0.4f); // �ִϸ��̼� �ð�

        foreach (Transform enemy in multipleAttackTargets)
        {
            SetEnemyState(enemy);
        }

        yield return new WaitForSeconds(0.2f); // �ִϸ��̼� �ð�

        // �ִϸ��̼�
        StartAnimation("Idle", true, 1.5f);
    }
}
