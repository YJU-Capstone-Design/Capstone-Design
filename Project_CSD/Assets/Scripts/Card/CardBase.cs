using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{   
    public enum ItemTYPE { Buff, Spell, Heal }

    [Header("# ItemState")]
    public ItemTYPE itemTYPE;
    public int ItemID;
    public float Heal;
    public float SpeedUp;
    public float PowerUp;
    public float Critical;
    public float BuffTime;

    public void Awake()
    {
        
    }

    private void CallCardData()
    {

    }
}
