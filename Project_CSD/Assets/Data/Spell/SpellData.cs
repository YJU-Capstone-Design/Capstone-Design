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
    private SpellTypes spellType;
    public SpellTypes SpellType { get { return spellType; } }

    [Header("# Spell State")]
    [SerializeField] // ������ ID
    private int spellID;
    public int SpellID { get { return spellID; } }

    [SerializeField] // ȸ��
    private string spellName;
    public string SpellName { get { return spellName; } }

    [SerializeField] // �ʿ� �ڽ�Ʈ
    private int cost;
    public int Cost { get { return cost; } }

    [SerializeField] // ���� ���ӽð�
    private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] // (���ݽ��� ����) �������
    private float damage;
    public float Damage { get { return damage; } }

    [SerializeField] // �ִ� HP ����
    private float maxHpUp;
    public float MaxHpUp { get { return maxHpUp; } }

    [SerializeField] // ���ݷ� ����
    private float powerUp;
    public float PowerUp { get { return powerUp; } }

    [SerializeField] // �̵��ӵ� ����
    private float moveSpeedUp;
    public float MoveSpeedUp { get { return moveSpeedUp; } }

    [SerializeField] // ���� ������ ���� (= ���ݼӵ� ����)
    private float attackTimeDown;
    public float AttackTimeDown { get { return attackTimeDown; } }
}
