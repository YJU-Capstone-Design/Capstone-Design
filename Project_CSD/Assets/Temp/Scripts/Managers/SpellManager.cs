using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    Spell spell;

    private void Awake()
    {
        spell = GetComponent<Spell>();
    }

    public void UsingCard()
    {
        if (spell.spellType != SpellBase.SpellTypes.Attack) // °ø°Ý ½ºÆçÀÌ ¾Æ´Ò ¶§
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
