using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static CardBase;
using static UnityEditor.Progress;

public class PlayerCard : CardBase
{
    
    [Header("# Item Setting")]
    public List<CardData> cards = new List<CardData>();
    public int value;
    public float power;

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        int ran = Random.Range(0, cards.Count);
        CallCardData(ran);
    }

    public void CallCardData(int type)
    {
        // ¼öÄ¡°ª
        ItemID = cards[type].ItemID;
        Heal = cards[type].Heal;
        SpeedUp = cards[type].SpeedUp;
        PowerUp = cards[type].PowerUp;
        BuffTime = cards[type].BuffTime;

        value = ItemID;
        Debug.Log(ItemID);
    }
}
