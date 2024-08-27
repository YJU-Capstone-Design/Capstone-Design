using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnitBase;

public class Unit : UnitBase
{
    [Header("# Spell Setting")]
    public List<UnitData> units = new List<UnitData>();
    public UnitData data;
    public Image cardImg;
    public TextMeshProUGUI unitCost;
    public TextMeshProUGUI unitText;
    public void OnEnable()
    {
        System.Random random = new System.Random();
        CallUnitData(random.Next(0, units.Count));
    }

    public void CallUnitData(int index)
    {
        // Unit Type
        unitType = (UnitTypes)units[index].UnitType;
        data = units[index];
        // Unit Info
        unitID = units[index].UnitID;
        unitName = units[index].UnitName;
        cost = units[index].Cost;

        // Unit Status
        health = units[index].Health;
        power = units[index].Power;
        attackTime = units[index].AttackTime;
        moveSpeed = units[index].MoveSpeed;
        cardImg.sprite = units[index].Unit_CardImg;
        unitCost.text = units[index].Cost.ToString();
        unitText.text = units[index].UnitName;
    }
    public void SetItemInfo()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        ItemInfo.instance.OpenInfoUnit(data);
    }
}
