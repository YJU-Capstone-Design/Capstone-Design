using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBase : MonoBehaviour
{
    public enum ItemTYPE { Attack, Buff, Debuff }

    [Header("# ItemState")]
    public ItemTYPE itemTYPE;
    public int itemID;
    public string itemName;
    public int cost;
    public float damage;
    public float maxHpUp;
    public float powerUp;
    public float speedUp;
    public float duration;
}
