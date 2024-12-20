using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpellBase;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Object/Spell Data")]
public class SpellData : ScriptableObject
{
    public enum SpellTypes { Attack, Buff, Debuff }

    [Header("# Spell Type")]
    [SerializeField]
    public SpellTypes spellType;
    public SpellTypes SpellType { get { return spellType; } }

    [Header("# Spell State")]
    [SerializeField] // 아이템 ID
    private int spellID;
    public int SpellID { get { return spellID; } }

    [SerializeField] // 이름
    private string spellName;
    public string SpellName { get { return spellName; } }

    [SerializeField] // 필요 코스트
    private int cost;
    public int Cost { get { return cost; } }

    [SerializeField] // 버프 지속시간
    private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] // (공격스펠 한정) 대미지값
    private float damage;
    public float Damage { get { return damage; } }

    [SerializeField] // 최대 HP 증가
    private float maxHpUp;
    public float MaxHpUp { get { return maxHpUp; } }

    [SerializeField] // 공격력 증가
    private float powerUp;
    public float PowerUp { get { return powerUp; } }

    [SerializeField] // 이동속도 증가
    private float moveSpeedUp;
    public float MoveSpeedUp { get { return moveSpeedUp; } }

    [SerializeField] // 공격 딜레이 감소 (= 공격속도 증가)
    private float attackTimeDown;
    public float AttackTimeDown { get { return attackTimeDown; } }

    [SerializeField] // 아이템 타입 유닛||스펠 1 = 유닛 2 = 스펭
    private int item_Type;
    public float Item_Type { get { return item_Type; } }

    [SerializeField] // 스펠 카드 이미지
    private Sprite spell_CardImg;
    public Sprite Spell_CardImg { get { return spell_CardImg; } }

    [SerializeField] // 스펠 카드 효과 Text
    private string spell_Effect;
    public string Spell_Effect { get { return spell_Effect; } }
}
