using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public void UsingCard()
    {
        if (unit.unitType != UnitBase.unitTypes.Enemy) // ���� ������ �ƴ� ��
        {
            Debug.Log("Using Buff or Debuff Spell");
            CardManager.Instance.Buff_Status(spell);
        }
        else // ���� ������ ��
        {
            Debug.Log("Using Attack Spell");
        }
        BattleManager.Instance.CardShuffle();
    }
}