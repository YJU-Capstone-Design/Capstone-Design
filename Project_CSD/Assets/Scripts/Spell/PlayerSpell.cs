using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static SpellBase;

public class PlayerSpell : SpellBase
{

    [Header("# Item Setting")]
    public List<SpellData> spell = new List<SpellData>();
    public int value;
    public float power;

    public void OnEnable()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, spell.Count);
        CallCardData(randomValue);
    }

    public void CallCardData(int type)
    {
        // ¼öÄ¡°ª
        itemID = spell[type].ItemID;
        itemName = spell[type].ItemName;
        cost = spell[type].Cost;
        damage = spell[type].Damage;
        speedUp = spell[type].SpeedUp;
        powerUp = spell[type].PowerUp;
        duration = spell[type].Duration;

        value = itemID;
        Debug.Log(itemID);
    }
}
