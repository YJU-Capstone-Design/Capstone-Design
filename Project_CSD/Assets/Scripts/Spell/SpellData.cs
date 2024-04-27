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
    [SerializeField] // ������ ID
    private int itemID;
    public int ItemID { get { return itemID; } }

    [SerializeField] // ������ �̸�
    private string itemName;
    public string ItemName { get { return itemName; } }

    [SerializeField] // �ʿ� �ڽ�Ʈ
    private int cost;
    public int Cost { get { return cost; } }

    [SerializeField] // �ʿ� �ڽ�Ʈ
    private float damage;
    public float Damage { get { return damage; } }

    [SerializeField] // �ִ� HP ����
    private float maxHpUp;
    public float MaxHpUp { get { return maxHpUp; } }

    [SerializeField] // ���ݷ� ����
    private float powerUp;
    public float PowerUp { get { return powerUp; } }

    [SerializeField] // �ӵ� ����
    private float speedUp;
    public float SpeedUp { get { return speedUp; } }

    [SerializeField] // ���� ���ӽð�
    private float duration;
    public float Duration { get { return duration; } }
}