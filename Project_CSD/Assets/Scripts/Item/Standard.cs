using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standad : MonoBehaviour
{   
    public enum ItemTYPE { Buff, Spell, Heal }

    [Header("# ItemState")]
    public ItemTYPE UpState;
    public int ItemID;
    public float Heal;
    public float SpeedUp;
    public float PowerUp;
    public float Critical;
    public float BuffTime;
}
