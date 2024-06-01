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
        if (unit.unitType != UnitBase.unitTypes.Enemy) // °ø°Ý ½ºÆçÀÌ ¾Æ´Ò ¶§
        {
            Debug.Log("Using Buff or Debuff Spell");
            CardManager.Instance.Buff_Status(spell);
        }
        else // °ø°Ý ½ºÆçÀÏ ¶§
        {
            Debug.Log("Using Attack Spell");
        }
        BattleManager.Instance.CardShuffle();
    }
}