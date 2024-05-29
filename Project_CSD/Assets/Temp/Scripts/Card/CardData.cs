using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardBase;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Object/Card Data")]
public class CardData : ScriptableObject
{
    public enum ItemTYPE { ATK, CC, SP }

    [Header("# Card State")]
    [SerializeField] // ������ ID
    private int itemID;
    public int ItemID { get { return itemID; } }

    [SerializeField] // ȸ��
    private float heal;
    public float Heal { get { return heal; } }

    [SerializeField] // �ֹ�
    private float speedUp;
    public float SpeedUp { get { return  speedUp; } }

    [SerializeField] // ���� ���ݷ�
    private float powerUp;
    public float PowerUp { get { return powerUp; } }

    [SerializeField] // ���� ���ӽð�
    private float buffTime;
    public float BuffTime { get { return buffTime; } }

    [SerializeField]
    private float critical;
    public float Critical { get { return critical; } }
}
