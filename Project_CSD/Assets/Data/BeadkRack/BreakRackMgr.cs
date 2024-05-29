using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BreakRackMgr : Singleton<BreakRackMgr>
{
    [Header("Bread오브젝트 정보")]
    public TextMeshProUGUI breadRack_Name;
    public TextMeshProUGUI breadRack_No;
    public TextMeshProUGUI breadRack_Cost;
    public TextMeshProUGUI breadRack_Level;
    public Image breadRack_Img;
    public float breadRack_Stats;
    public TextMeshProUGUI breadRack_InfoTxt;

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
    public void BreadInfoValue(BreadRack_Data data)
    {
        breadRack_Name.text = data.BreadRack_Name;
        breadRack_Cost.text = data.BreadRack_Cost.ToString();
        breadRack_Level.text = "Lv "+data.BreadRack_Level.ToString();
        breadRack_Img.sprite = data.BreadRack_Img;
        breadRack_InfoTxt.text = data.BreadRack_InfoTxt;
    }


}
