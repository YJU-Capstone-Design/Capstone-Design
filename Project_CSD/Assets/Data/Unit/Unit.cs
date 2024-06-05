using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitBase;

public class Unit : UnitBase
{
    [Header("# Spell Setting")]
    public List<UnitData> units = new List<UnitData>();

    public void OnEnable()
    {
        System.Random random = new System.Random();
        CallUnitData(random.Next(0, units.Count));
    }

    public void CallUnitData(int index)
    {
        // Unit Type
        unitType = (UnitTypes)units[index].UnitType;

        // Unit Info
        unitID = units[index].UnitID;
        unitName = units[index].UnitName;
        cost = units[index].Cost;

        // Unit Status
        health = units[index].Health;
        power = units[index].Power;
        attackTime = units[index].AttackTime;
        moveSpeed = units[index].MoveSpeed;
    }
}
