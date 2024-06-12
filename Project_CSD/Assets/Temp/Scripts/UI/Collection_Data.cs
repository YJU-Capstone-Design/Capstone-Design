using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Collection_Data : MonoBehaviour
{
    public UnitData data;
    public SpellData spelldata;
    public int id;

    public Image icon;

    public void Init(UnitData data)
    {
        this.data = data;
        icon.sprite = data.Unit_Img;
        id = data.UnitID;

        gameObject.SetActive(true);

    }
    public void SpellInit(SpellData spelldata)
    {
        this.spelldata = spelldata;
        icon.sprite = spelldata.Spell_CardImg;
        id = spelldata.SpellID;

        gameObject.SetActive(true);

    }
}