using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellBase : MonoBehaviour
{
    public enum SpellTypes { Attack, Buff, Debuff }

    [Header("# Spell Type")]
    public SpellTypes spellType;

    [Header("# Spell State")]
    public int spellID;
    public string spellName;
    public int cost;

    // 버프 지속시간
    public float duration;

    // (공격스펠 한정) 대미지값
    public float damage;

    // 스테이터스 버프
    public float maxHpUp; // 최대 HP 증가
    public float powerUp; // 공격력 증가
    public float attackSpeedUp; // 공격속도 증가
    public float moveSpeedUp; // 이동속도 증가
}
