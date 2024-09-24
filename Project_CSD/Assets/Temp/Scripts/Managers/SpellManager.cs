using System.Collections;
using System.Collections.Generic;
using System.Xml;
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

        // usingCount ���̺� �ش� ID �� �÷��� count ���� +1, �ش� ID �÷��� ������ ���� �߰�
        XmlNodeList cardData = DBConnect.Select("usingCount", $"WHERE cardID = {spell.data.SpellID}");

        if (cardData != null)
        {
            DBConnect.UpdateOriginal($"UPDATE usingCount SET count = count + 1 WHERE cardID = {spell.data.SpellID}");
        }
        else
        {
            // ���� �÷� �߰�
            DBConnect.Insert("usingCount", $"{spell.data.SpellID}, 1");
        }
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
