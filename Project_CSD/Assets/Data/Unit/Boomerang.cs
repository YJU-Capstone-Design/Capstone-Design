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

        // 애니메이션
        StartAnimation("Attack", false, 1f);

        yield return new WaitForSeconds(0.4f); // 애니메이션 시간

        // 총 2 번 공격
        foreach (Transform enemy in multipleAttackTargets)
        {
            SetEnemyState(enemy);
        }

        yield return new WaitForSeconds(0.4f); // 애니메이션 시간

        foreach (Transform enemy in multipleAttackTargets)
        {
            SetEnemyState(enemy);
        }

        yield return new WaitForSeconds(0.2f); // 애니메이션 시간

        // 애니메이션
        StartAnimation("Idle", true, 1.5f);
    }
}
