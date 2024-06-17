using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitBase;

public class MainWall : MonoBehaviour
{
    Collider2D col;

    public void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void Update()
    {
        if(BattleManager.Instance.curHealth <= 0)
        {
            StartCoroutine(Break());
        }
    }

    IEnumerator Break()
    {
        // �� �ڸ����� �ִϸ��̼� �ʿ�

        BattleManager.Instance.battleState = BattleManager.BattleState.Lose;

        yield return new WaitForSeconds(1f);

        Debug.Log("Break");
        col.enabled = false;
        gameObject.SetActive(false);
    }
}
