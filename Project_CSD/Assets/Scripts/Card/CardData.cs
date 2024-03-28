using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardBase;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Object/Card Data")]
public class CardData : ScriptableObject
{
    public enum ItemTYPE { ATK, CC, SP }

    [Header("# Card State")]
    [SerializeField] // 아이템 ID
    private int itemID;
    public int ItemID { get { return itemID; } }

    [SerializeField] // 회복
    private float heal;
    public float Heal { get { return heal; } }

    [SerializeField] // 주문
    private float speedUp;
    public float SpeedUp { get { return  speedUp; } }

    [SerializeField] // 유닛 공격력
    private float powerUp;
    public float PowerUp { get { return powerUp; } }

    [SerializeField] // 버프 지속시간
    private float buffTime;
    public float BuffTime { get { return buffTime; } }

    [SerializeField]
    private float critical;
    public float Critical { get { return critical; } }
}
