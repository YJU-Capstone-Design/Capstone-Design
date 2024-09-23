using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SpellBase;
using static UnityEngine.UI.CanvasScaler;

public class CardManager : Singleton<CardManager>
{
    public BattleData battleData;

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
        if (battleData == null)
        {
            GameObject go = GameObject.Find("BattleData");
            battleData = go.GetComponent<BattleData>();
        }

        units = new List<GameObject>();
        enemys = new List<GameObject>();
    }

    private void Update()
    {
        units = BattleData.Instance.units;
        enemys = BattleData.Instance.enemys;
    }

    public void Buff_Status(Spell spell)
    {
        usingSpell = spell;

        if (usingSpell.spellType == SpellTypes.Buff)
        {
            foreach (GameObject unit in units)
            {
                StartCoroutine(Buff_Logic_Unit(unit, spell));
            }
        } else if (usingSpell.spellType == SpellTypes.Debuff) {
            foreach (GameObject enemy in enemys)
            {
                StartCoroutine(Buff_Logic_Enemy(enemy, spell));
            }
        }

        BattleManager.Instance.CardShuffle(false);
    }

    public IEnumerator Buff_Logic_Unit(GameObject unit, Spell spell)
    {
        PlayerUnit status = unit.GetComponent<PlayerUnit>();
        //float maxHpUpPoint = status.initialHealth * (spell.maxHpUp * 0.01f);
        float powerUpPoint = status.initialPower * (spell.powerUp * 0.01f);
        float attackTimeDownPoint = status.initialAttackTime * (spell.attackTimeDown * 0.01f);
        float moveSpeedUpPoint = status.initialMoveSpeed * (spell.moveSpeedUp * 0.01f);

        float actualAttackTimeDownPoint = (status.attackTime - 1.5f) <= attackTimeDownPoint ? (status.attackTime - 1.5f) : attackTimeDownPoint;

        //status.health += maxHpUpPoint;
        status.power += powerUpPoint;
        status.attackTime -= actualAttackTimeDownPoint;
        status.moveSpeed += moveSpeedUpPoint;
        Debug.Log("power = " + status.power + ", attackTime = " + status.attackTime + ", moveSpeed = " + status.moveSpeed + ", spellID = " + spell.spellName);
        unit.GetComponent<PlayerUnit>().Buff_Effect(spell.spellType, true, spell.spellID);

        float time = 0;

        while (time <= spell.duration)
        {
            yield return new WaitForSeconds(1f);
            time += 1f;
            Debug.Log(status.name + " 버프 지속중");
        }

        //status.health -= maxHpUpPoint;
        status.power -= powerUpPoint;
        status.attackTime += actualAttackTimeDownPoint;
        status.moveSpeed -= moveSpeedUpPoint;
        Debug.Log("power = " + status.power + ", attackTime = " + status.attackTime + ", moveSpeed = " + status.moveSpeed + ", spellID = " + spell.spellName);
        unit.GetComponent<PlayerUnit>().Buff_Effect(spell.spellType, false, spell.spellID);
        Debug.Log(status.name + " 버프 종료");
    }

    public IEnumerator Buff_Logic_Enemy(GameObject enemy, Spell spell)
    {
        EnemyUnit status = enemy.GetComponent<EnemyUnit>();
        //float maxHpUpPoint = status.initialHealth * (spell.maxHpUp * 0.01f);
        float powerUpPoint = status.initialPower * (spell.powerUp * 0.01f);
        float attackTimeDownPoint = status.initialAttackTime * (spell.attackTimeDown * 0.01f);
        float moveSpeedUpPoint = status.initialMoveSpeed * (spell.moveSpeedUp * 0.01f);

        float actualAttackTimeDownPoint = (status.attackTime - 1.5f) <= attackTimeDownPoint ? (status.attackTime - 1.5f) : attackTimeDownPoint;

        //status.health += maxHpUpPoint;
        status.power += powerUpPoint;
        status.attackTime -= actualAttackTimeDownPoint;
        status.moveSpeed += moveSpeedUpPoint;
        Debug.Log("power = " + status.power + ", attackTime = " + status.attackTime + ", moveSpeed = " + status.moveSpeed + ", spellID = " + spell.spellName);
        enemy.GetComponent<EnemyUnit>().Buff_Effect(spell.spellType, true, spell.spellID);

        float time = 0;

        while (time <= spell.duration)
        {
            yield return new WaitForSeconds(1f);
            time += 1f;
            Debug.Log(status.name + " 디버프 지속중");
        }

        //status.health -= maxHpUpPoint;
        status.power -= powerUpPoint;
        status.attackTime += actualAttackTimeDownPoint;
        status.moveSpeed -= moveSpeedUpPoint;
        Debug.Log("power = " + status.power + ", attackTime = " + status.attackTime + ", moveSpeed = " + status.moveSpeed + ", spellID = " + spell.spellName);
        enemy.GetComponent<EnemyUnit>().Buff_Effect(spell.spellType, false, spell.spellID);
        Debug.Log(status.name +" 디버프 종료");
    }
}