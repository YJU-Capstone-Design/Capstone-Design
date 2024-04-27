using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : Singleton<SpellManager>
{
    public List<GameObject> units;
    public List<GameObject> enemys;

    [Header("Using Card")]
    public Transform poolObj;
    PlayerSpell playerSpell;

    private void Awake()
    {
        playerSpell = GetComponent<PlayerSpell>();
        units = new List<GameObject>();
        enemys = new List<GameObject>();
    }

    public void Buff_Logic(int value)
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
