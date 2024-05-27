using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    PlayerCard playerCard;
    PlayerUnit playerUnit;

    private void Awake()
    {
        playerCard = GetComponent<PlayerCard>();
        playerUnit = GetComponent<PlayerUnit>();
    }

    public void UsingCard()
    {
        int value = this.playerCard.value;

        switch (value)
        {
            case 20000:
                CardManger.Instance.Buff(value);
                Debug.Log("ATK_UP");
                break;
            case 20001:
                CardManger.Instance.Buff(value);
                Debug.Log("SPEED_UP");
                break;
            case 22001:
                CardManger.Instance.Buff(value);
                Debug.Log("HEAL");
                break;
        }
    }
}
