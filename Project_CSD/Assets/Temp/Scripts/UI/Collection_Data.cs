using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Collection_Data : MonoBehaviour
{
    public UnitData data;
    public SpellData spelldata;
    public int id;
    public TextMeshProUGUI spelltext;
    public Image icon;


    [Header("Spell")]//½ºÆçÄ«µå
    public Image bg;
    public Image frame;
    public GameObject panel;
    public Sprite sp_bg;
    public Sprite sp_frame;
    

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
    
        spelltext.text = spelldata.SpellName;
        gameObject.SetActive(true);
        bg.sprite = sp_bg;
        frame.sprite = sp_frame;
        panel.SetActive(true);
    }
}