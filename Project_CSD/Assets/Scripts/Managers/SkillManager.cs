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
                CardManger.Instance.ATK_UP();
                Debug.Log("ATK_UP");
                break;
            case 20001:
                Debug.Log("SPEED_UP");
                break;
            case 22001:
                Debug.Log("HEAL");
                break;
        }
    }
}
