using Spine.Unity;
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
    public SkeletonDataAsset unit_anim;
    public TextMeshProUGUI unitCost;
    public TextMeshProUGUI unitText;

    private Vector3 originalScale; // 원래 스케일 저장

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnEnable()
    {
        System.Random random = new System.Random();
        CallUnitData(random.Next(0, units.Count));
    }

    public void CallUnitData(int index)
    {
        unitType = (UnitTypes)units[index].UnitType;
        data = units[index];
        unitID = units[index].UnitID;
        unitName = units[index].UnitName;
        cost = units[index].Cost;
        health = units[index].Health;
        power = units[index].Power;
        attackTime = units[index].AttackTime;
        moveSpeed = units[index].MoveSpeed;
        cardImg.sprite = units[index].Unit_CardImg;
        unitCost.text = units[index].Cost.ToString();
        unitText.text = units[index].UnitName;
        unit_anim = units[index].Unit_skeletonData;
    }

    public void SetItemInfo()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        ItemInfo.instance.OpenInfoUnit(data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 드래그 중이면 스케일 변경 무시
        if (UnitManager.isDraggingAny) return;

        Debug.Log("Pointer entered");
        ItemInfo.instance.OpenInfoUnit(data);

        Vector3 newScale = new Vector3(
            originalScale.x + 0.2f,
            originalScale.y + 0.2f,
            originalScale.z);
        transform.localScale = newScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited");
        // 현재값 기준이 아닌 원래 스케일로 정확히 복구
        transform.localScale = originalScale;
    }
}