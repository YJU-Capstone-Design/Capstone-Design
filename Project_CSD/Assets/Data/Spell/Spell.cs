using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpellBase;

public class Spell : SpellBase
{
    [Header("# Spell Setting")]
    public List<SpellData> spells = new List<SpellData>();

    public void OnEnable()
    {
        System.Random random = new System.Random();
        CallSpellData(random.Next(0, spells.Count));
    }

    public void CallSpellData(int index)
    {
        // Spell Type
        spellType = (SpellTypes) spells[index].SpellType;

        // Spell Info
        spellID = spells[index].SpellID;
        spellName = spells[index].SpellName;
        cost = spells[index].Cost;
        duration = spells[index].Duration;

        // Spell Ability
        damage = spells[index].Damage;
        maxHpUp = spells[index].MaxHpUp;
        powerUp = spells[index].PowerUp;
        attackTimeDown = spells[index].AttackTimeDown;
        moveSpeedUp = spells[index].MoveSpeedUp;
    }
}
