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
        if (spell.spellType != SpellBase.SpellTypes.Attack) // 공격 스펠이 아닐 때
        {
            Debug.Log("Using Buff or Debuff Spell");
            CardManager.Instance.Buff_Status(spell);
        }
        else // 공격 스펠일 때
        {
            Debug.Log("Using Attack Spell");
        }
        BattleManager.Instance.CardShuffle(false);

        // usingCount 테이블에 해당 ID 의 컬럼에 count 값에 +1, 해당 ID 컬럼이 없으면 먼저 추가
        XmlNodeList cardData = DBConnect.Select("usingCount", $"WHERE cardID = {spell.data.SpellID}");

        if (cardData != null)
        {
            DBConnect.UpdateOriginal($"UPDATE usingCount SET count = count + 1 WHERE cardID = {spell.data.SpellID}");
        }
        else
        {
            // 새로 컬럼 추가
            DBConnect.Insert("usingCount", $"{spell.data.SpellID}, 1");
        }
    }

    public void Buy()
    {
        // uiMgr.cost와 unitData.Cost를 비교하여 구매 가능한지 확인
        if (UiManager.Instance != null && UiManager.Instance.cost >= spell.cost)
        {
            // 코스트를 차감하고 유닛을 스폰
            UiManager.Instance.cost -= spell.cost;
            UsingCard(spell);
        }
        else
        {
            Debug.Log("돈이 없다! 게이게이야! ");
        }
    }
}
