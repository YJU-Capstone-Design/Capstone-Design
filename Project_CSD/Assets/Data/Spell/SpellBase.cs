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

    // ���� ���ӽð�
    public float duration;

    // (���ݽ��� ����) �������
    public float damage;

    // �������ͽ� ����
    public float maxHpUp; // �ִ� HP ����
    public float powerUp; // ���ݷ� ����
    public float attackSpeedUp; // ���ݼӵ� ����
    public float moveSpeedUp; // �̵��ӵ� ����
}
