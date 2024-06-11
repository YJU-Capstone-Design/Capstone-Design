using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gacha : MonoBehaviour
{
    

    [SerializeField] GameObject screen;
    [SerializeField] Image obj_Img;
    [SerializeField] Sprite basic_Img;
    [SerializeField] private UnitData[] unit_DB;

    [SerializeField] GameObject holding;
    
    public void Init(UnitData data)
    {
        obj_Img.sprite = data.Unit_Img;
        gameObject.SetActive(true);
    }
}
