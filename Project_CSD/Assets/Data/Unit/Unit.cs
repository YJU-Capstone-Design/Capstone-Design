using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnitBase;

public class Unit : UnitBase, IPointerEnterHandler, IPointerExitHandler
{
    [Header("# Spell Setting")]
    public List<UnitData> units = new List<UnitData>();
    public UnitData data;
    public Image cardImg;
    public TextMeshProUGUI unitCost;
    public TextMeshProUGUI unitText;

    public void OnEnable()
    {
        System.Random random = new System.Random();
        CallUnitData(random.Next(0, units.Count));
    }

    public void CallUnitData(int index)
    {
        // Unit Type
        unitType = (UnitTypes)units[index].UnitType;
        data = units[index];
        // Unit Info
        unitID = units[index].UnitID;
        unitName = units[index].UnitName;
        cost = units[index].Cost;

        // Unit Status
        health = units[index].Health;
        power = units[index].Power;
        attackTime = units[index].AttackTime;
        moveSpeed = units[index].MoveSpeed;
        cardImg.sprite = units[index].Unit_CardImg;
        unitCost.text = units[index].Cost.ToString();
        unitText.text = units[index].UnitName;
    }

    public void SetItemInfo()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        ItemInfo.instance.OpenInfoUnit(data);
    }

    // Implement IPointerEnterHandler's OnPointerEnter method
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered");
        ItemInfo.instance.OpenInfoUnit(data);
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


}
