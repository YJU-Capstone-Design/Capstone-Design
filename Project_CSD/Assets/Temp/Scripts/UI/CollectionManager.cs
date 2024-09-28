using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.Xml;
using System;

public class CollectionManager : MonoBehaviour
{
    [Header("# Unit Collection")]
    [SerializeField] GameObject unitCollectionUI;
    [SerializeField] GameObject spellCollectionUI;
    [SerializeField] TextMeshProUGUI unitNameText;
    [SerializeField] TextMeshProUGUI unitHPText;
    [SerializeField] TextMeshProUGUI unitPowerText;
    [SerializeField] TextMeshProUGUI unitCostText;
    [SerializeField] TextMeshProUGUI unitSpeedText;
    [SerializeField] TextMeshProUGUI unitAtkSpeedText;
    [SerializeField] TextMeshProUGUI usePercentText;
    [SerializeField] GameObject unitGraphic;
    [SerializeField] GameObject animButtons;
    [SerializeField] TextMeshProUGUI attackAnimText;


    private void Awake()
    {
        // UnitCollectionClear();
    }

    public void OpenUI(string UIName)
    {
        switch (UIName)
        {
            case "unitCollection":
                unitCollectionUI.SetActive(true);
                spellCollectionUI.SetActive(false);
                break;
            case "spellCollection":
                unitCollectionUI.SetActive(false);
                spellCollectionUI.SetActive(true);
                UnitCollectionClear();
                break;
        }

        // 사운드
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
    }

    public void CloseCollection()
    {
        UnitCollectionClear();

        // 사운드
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
    }

    void UnitCollectionClear()
    {
        unitCollectionUI.SetActive(false);
        unitNameText.text = "";
        unitHPText.text = "";
        unitPowerText.text = "";
        unitCostText.text = "";
        unitSpeedText.text = "";
        unitAtkSpeedText.text = "";
        usePercentText.text = "";
        attackAnimText.text = "공격";
        animButtons.SetActive(false);
        unitGraphic.SetActive(false);

        if (unitGraphic.GetComponent<SkeletonGraphic>().SkeletonDataAsset != null)
        {
            unitGraphic.GetComponent<SkeletonGraphic>().SkeletonDataAsset.Clear();
        }
    }

    // 유닛 도감 버튼(유닛 카드) 함수
    public void GetUnitInfo(UnitData unitData)
    {
        unitNameText.text = unitData.UnitName;
        unitHPText.text = unitData.Health.ToString();
        unitPowerText.text = unitData.Power.ToString();
        unitCostText.text = unitData.Cost.ToString();
        unitSpeedText.text = unitData.MoveSpeed.ToString();
        unitAtkSpeedText.text = unitData.AttackTime.ToString();
        usePercentText.text = GetUsePercentage(unitData.UnitID);
        animButtons.SetActive(true);

        // 유닛 Spine UI
        unitGraphic.SetActive(false);
        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();
        unitSkeletonGraphic.skeletonDataAsset = unitData.Unit_skeletonData;
        if (unitData.UnitName == "에그볼")
        {
            unitSkeletonGraphic.startingAnimation = "idle_3unit";
        }
        else
        {
            unitSkeletonGraphic.startingAnimation = "Idle";
        }
        unitSkeletonGraphic.Initialize(true); // skeletonDataAsset 를 ReLoad
        unitSkeletonGraphic.startingLoop = true;
        unitGraphic.SetActive(true);

        if(unitData.UnitName == "빵게")
        {
            attackAnimText.text = "방어";
        }
        else
        {
            attackAnimText.text = "공격";
        }
    }

    // SkeletonGraphic 애니메이션 버튼
    public void UnitAnimButton(string animName)
    {
        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();

        // 다른 애니메이션을 가진 unit 수정
        switch ((unitNameText.text, animName))
        {
            case ("빵게", "Attack"):
                unitSkeletonGraphic.startingAnimation = "Defense_start";
                break;
            case ("에그볼", "Idle"):
                unitSkeletonGraphic.startingAnimation = "idle_3unit";
                break;
            case ("에그볼", "Walk"):
                unitSkeletonGraphic.startingAnimation = "walk_3unit";
                break;
            case ("에그볼", "Attack"):
                unitSkeletonGraphic.startingAnimation = "attack_3unit";
                break;
            case ("에그볼", "Win"):
                unitSkeletonGraphic.startingAnimation = "win_3unit";
                break;
            case ("에그볼", "Die"):
                unitSkeletonGraphic.startingAnimation = "die_3unit";
                break;
            default:
                unitSkeletonGraphic.startingAnimation = animName;
                break;
        }

        unitSkeletonGraphic.Initialize(true); // skeletonDataAsset 를 ReLoad
        unitSkeletonGraphic.startingLoop = true;
    }


    // 카드 사용 퍼센트 출력 함수
    string GetUsePercentage(int id)
    {
        float allUseCount = 0; // 모든 카드의 사용 횟수
        float useCount = 0; // 선택한 카드의 사용 횟수
        float percent = 0;

        XmlNodeList allCardData = DBConnect.SelectOriginal("usingCount", "SELECT * FROM usingCount");

        for(int i = 0; i < allCardData.Count; i++)
        {
            allUseCount += int.Parse(allCardData[i].SelectSingleNode("count").InnerText);
        }

        XmlNodeList selectedCard = DBConnect.SelectOriginal("usingCount", $"SELECT count FROM usingCount WHERE cardID = {id};");
        
        if(selectedCard != null)
        {
            useCount = int.Parse(selectedCard[0].InnerText);
        }
        else
        {
            DBConnect.Insert("usingCount", $"{id}, 0");
            useCount = 0;
        }

        percent = (useCount / allUseCount) * 100;
        percent = (float)Math.Round(percent, 2); // 소수점 2자리 반올림

        Debug.Log($"useCount : {useCount}, allUseCount : {allUseCount}, percent : {percent}");

        // 텍스트 return
        return $"{percent}%";
    }
}
