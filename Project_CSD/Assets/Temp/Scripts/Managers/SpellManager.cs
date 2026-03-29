using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellManager : MonoBehaviour, IPointerClickHandler
{
    Spell spell;

    private void Awake()
    {
        spell = GetComponent<Spell>();
    }

    // ────────────────────────────────────────────
    // 카드 터치 시 즉시 코스트 확인 후 사용
    // ────────────────────────────────────────────
    public void OnPointerClick(PointerEventData eventData)
    {
        Buy();
    }

    public void Buy()
    {
        if (UiManager.Instance == null)
            return;

        if (UiManager.Instance.cost >= spell.cost)
        {
            UiManager.Instance.cost -= spell.cost;
            UsingCard(spell);
        }
        else
        {
            Debug.Log("돈이 없다!");
            // 필요하면 여기서 잔액 부족 UI 연출 추가
        }
    }

    private void UsingCard(Spell spell)
    {
        if (spell.spellType != SpellBase.SpellTypes.Attack)
        {
            Debug.Log("Using Buff or Debuff Spell");
            CardManager.Instance.Buff_Status(spell);
        }
        else
        {
            Debug.Log("Using Attack Spell");
        }

        BattleManager.Instance.CardShuffle(false);
    }
}