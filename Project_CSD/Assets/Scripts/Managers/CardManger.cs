using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManger : MonoBehaviour
{
    public static CardManger instance;

    PlayerCard playerCard;

    private void Awake()
    {
        instance = this;

        playerCard = GetComponent<PlayerCard>();
    }


    public void UsingCard()
    {
        int value = this.playerCard.value;

        switch (value)
        {
            case 20000:
                Debug.Log("ATK_UP");
                break;
            case 20001:
                Debug.Log("SPD_UP");
                break;
            case 22001:
                Debug.Log("HEAL");
                break;
        }
    }
}
