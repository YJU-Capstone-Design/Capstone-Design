using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SpellBase;

public class Spell : SpellBase, IPointerEnterHandler, IPointerExitHandler
{
    [Header("# Spell Setting")]
    public List<SpellData> spells = new List<SpellData>();

    public Image cardImg;
    public TextMeshProUGUI spellCost;
    public TextMeshProUGUI spelltext;
    public SpellData data;

    public void OnEnable()
    {
        System.Random random = new System.Random();
        CallSpellData(random.Next(0, spells.Count));
      
    }

    public void CallSpellData(int index)
    {
        data = spells[index];
        // Spell Type
        spellType = (SpellTypes) spells[index].SpellType;

        // Spell Info
        spellID = spells[index].SpellID;
        spellName = spells[index].SpellName;
        cost = spells[index].Cost;
        duration = spells[index].Duration;

        // Spell Ability
        damage = spells[index].Damage;
        maxHpUp = spells[index].MaxHpUp;
        powerUp = spells[index].PowerUp;
        attackTimeDown = spells[index].AttackTimeDown;
        moveSpeedUp = spells[index].MoveSpeedUp;

        cardImg.sprite = spells[index].Spell_CardImg;
        spellCost.text = spells[index].Cost.ToString();
        spelltext.text = spells[index].SpellName;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered");
        ItemInfo.instance.OpenInfoSpell(data);
        Vector3 currentScale = transform.localScale;

        // 가로와 세로를 0.2씩 증가시킴
        Vector3 newScale = new Vector3(currentScale.x + 0.2f, currentScale.y + 0.2f, currentScale.z);

        // 새로운 스케일을 적용
        transform.localScale = newScale;
    }

    // Implement IPointerExitHandler's OnPointerExit method
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited");
        Vector3 currentScale = transform.localScale;

        // 가로와 세로를 0.2씩 증가시킴
        Vector3 newScale = new Vector3(currentScale.x - 0.2f, currentScale.y - 0.2f, currentScale.z);

        // 새로운 스케일을 적용
        transform.localScale = newScale;
    }


    public void SetItemInfo()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }

        ItemInfo.instance.OpenInfoSpell(data);
    }

    // 이 함수는 마우스가 오브젝트를 벗어날 때 호출됩니다.
    
}
