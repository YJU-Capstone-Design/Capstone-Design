using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SpellBase;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Object/Spell Data")]
public class SpellData : ScriptableObject
{
    public enum ItemTYPE { Attack, Buff, Debuff }

    [Header("# Card State")]
    [SerializeField] // 아이템 ID
    private int itemID;
    public int ItemID { get { return itemID; } }

    [SerializeField] // 아이템 이름
    private string itemName;
    public string ItemName { get { return itemName; } }

    [SerializeField] // 필요 코스트
    private int cost;
    public int Cost { get { return cost; } }

    [SerializeField] // 필요 코스트
    private float damage;
    public float Damage { get { return damage; } }

    [SerializeField] // 최대 HP 증가
    private float maxHpUp;
    public float MaxHpUp { get { return maxHpUp; } }

    [SerializeField] // 공격력 증가
    private float powerUp;
    public float PowerUp { get { return powerUp; } }

    [SerializeField] // 속도 증가
    private float speedUp;
    public float SpeedUp { get { return speedUp; } }

    [SerializeField] // 스펠 지속시간
    private float duration;
    public float Duration { get { return duration; } }
}