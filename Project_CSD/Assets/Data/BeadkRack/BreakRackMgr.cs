using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakRackMgr : Singleton<BreakRackMgr>
{
    public GameObject[] set_Item = new GameObject[8];
    public Transform set_Top;
    public Transform set_Bottom;
    
    public void SetItem(GameObject set)
    {
        for(int i=0; i<set_Item.Length; i++)
        {
            if (set_Item[i] == null && i<=4)
            {
                set_Item[i] = set;
            }
        }
    }



}
