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

    public void Buff(int value)
    {
       foreach (GameObject obj in units)
        {
            PlayerUnit unitLogic = obj.GetComponent<PlayerUnit>();
            PlayerUnit unitBuff = obj.GetComponentInChildren<PlayerUnit>();
            unitLogic.power += (unitLogic.power * 0.05f);
            Debug.Log(unitLogic.power);
            unitBuff.buff(value);
        }
        BattleManager.Instance.CardShuffle();
    }
}