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

    public void UsingCard(Spell spell)
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
        BattleManager.Instance.CardShuffle(false);
    }

    public void Buy()
    {
        // uiMgr.cost�� unitData.Cost�� ���Ͽ� ���� �������� Ȯ��
        if (UiManager.Instance != null && UiManager.Instance.cost >= spell.cost)
        {
            // �ڽ�Ʈ�� �����ϰ� ������ ����
            UiManager.Instance.cost -= spell.cost;
            UsingCard(spell);
        }
        else
        {
            Debug.Log("���� ����! ���̰��̾�! ");
        }
    }
}
