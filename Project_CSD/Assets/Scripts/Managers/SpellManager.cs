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
        if (spell.spellType != SpellBase.SpellTypes.Attack) // ���� ������ �ƴ� ��
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
