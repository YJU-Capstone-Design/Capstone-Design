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
        switch (spell.spellType)
        {
            case SpellBase.SpellTypes.Attack:
                Debug.Log("Using Attack Spell");
                break;
            case SpellBase.SpellTypes.Buff:
                Debug.Log("Using Buff Spell");
                CardManager.Instance.Buff_Status(spell);
                break;
            case SpellBase.SpellTypes.Debuff:
                Debug.Log("Using Debuff Spell");
                CardManager.Instance.Buff_Status(spell);
                break;
        }
        BattleManager.Instance.CardShuffle();
    }
}
