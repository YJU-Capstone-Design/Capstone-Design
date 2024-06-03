using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : Singleton<BattleData>
{
    public List<GameObject> units;
    public List<GameObject> enemys;

    private void Awake()
    {
        units = new List<GameObject>();
        enemys = new List<GameObject>();
    }
}
