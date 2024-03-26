using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManger : Singleton<CardManger>
{
    public static CardManger instance;

    public List<GameObject> units;
    public List<GameObject> enemys;

    [Header("Using Card")]
    public Transform poolObj;
    PlayerCard playerCard;
    private void Awake()
    {
        instance = this;

        playerCard = GetComponent<PlayerCard>();
        units = new List<GameObject>();
        enemys = new List<GameObject>();
    }


    public void UsingCard()
    {
        int value = this.playerCard.value;

        switch (value)
        {
            case 20000:
                ATK_UP();
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

    void ATK_UP()
    {
       foreach (GameObject obj in units)
        {
            PlayerUnit unitLogic = obj.GetComponent<PlayerUnit>();
            unitLogic.power += (unitLogic.power * 0.05f);
            Debug.Log(unitLogic.power);
        }
    }
}