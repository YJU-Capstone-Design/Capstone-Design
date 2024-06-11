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


    public void OnEnable()
    {
        int ran = Random.Range(0, 5);
        obj_Img.sprite = unit_DB[ran].Unit_Img;
    }
}
