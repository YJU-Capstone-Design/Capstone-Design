using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static SpellBase;

public class Spell : SpellBase
{
    [Header("# Spell Setting")]
    public List<SpellData> spells = new List<SpellData>();

    public Image cardImg;
    public TextMeshProUGUI spellCost;
    public TextMeshProUGUI spelltext;
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

        cardImg.sprite = spells[index].Spell_CardImg;
        spellCost.text = spells[index].Cost.ToString();
        spelltext.text = spells[index].SpellName;
    }
}
