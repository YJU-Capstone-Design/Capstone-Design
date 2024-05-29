using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpellBase;

public class CardManager : Singleton<CardManager>
{
    public List<GameObject> units;
    public List<GameObject> enemys;

    [Header("# Using Spell")]
    [SerializeField] private Spell usingSpell;

    [Header("# Spell Effect")]
    public List<GameObject> effect = new List<GameObject>();
    public GameObject effect_Attack;
    public GameObject effect_Buff;
    public GameObject effect_Debuff;

    private void Awake()
    {
        units = new List<GameObject>();
        enemys = new List<GameObject>();
    }

    public void Buff_Status(Spell spell)
    {
        usingSpell = spell;
        foreach (GameObject unit in units)
        {
            StartCoroutine(Buff_Logic(unit, spell));
        }
        // BattleManager.Instance.CardShuffle();
    }

    public IEnumerator Buff_Logic(GameObject unit, Spell spell)
    {
        PlayerUnit status = unit.GetComponent<PlayerUnit>();
        float maxHpUpPoint = status.initialHealth * (spell.maxHpUp * 0.01f);
        float powerUpPoint = status.initialPower * (spell.powerUp * 0.01f);
        float speedUpPoint = status.initialSpeed * (spell.speedUp * 0.01f);

        // status.health += maxHpUpPoint;
        status.power += powerUpPoint;
        status.speed += speedUpPoint;
        Debug.Log("power = " + status.power + ", speed = " + status.speed + ", spellID = " + spell.spellID);
        unit.GetComponent<PlayerUnit>().Buff_Effect(spell.spellType, true);

        float time = 0;

        while (time <= spell.duration)
        {
            yield return new WaitForSeconds(1f);
            time += 1f;
        }

        // status.health -= maxHpUpPoint;
        status.power -= powerUpPoint;
        status.speed -= speedUpPoint;
        Debug.Log("power = " + status.power + ", speed = " + status.speed);
        unit.GetComponent<PlayerUnit>().Buff_Effect(spell.spellType, false);
    }
}