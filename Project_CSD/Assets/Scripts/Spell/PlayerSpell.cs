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
        spellType = (SpellTypes) spell[type].SpellType;
        spellID = spell[type].SpellID;
        spellName = spell[type].SpellName;
        cost = spell[type].Cost;
        duration = spell[type].Duration;
        damage = spell[type].Damage;
        speedUp = spell[type].SpeedUp;
        powerUp = spell[type].PowerUp;
    }
}
