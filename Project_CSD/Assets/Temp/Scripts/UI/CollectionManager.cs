using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.Xml;
using System;
using System.Reflection;

public class CollectionManager : MonoBehaviour
{
    [Header("# Unit Collection")]
    [SerializeField] GameObject unitCollectionUI;
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
    Coroutine sorangAnim;
    [SerializeField] bool startSorangAnim = false;

    [Header("# Spell Collection")]
    [SerializeField] GameObject spellCollectionUI;
    [SerializeField] TextMeshProUGUI spellNameText;
    [SerializeField] TextMeshProUGUI spellCostText;
    [SerializeField] TextMeshProUGUI spellPercentText;
    [SerializeField] TextMeshProUGUI durationTimeText;
    [SerializeField] TextMeshProUGUI spellEffectText;
    [SerializeField] GameObject unitGraphic_Spell;
    [SerializeField] GameObject monster_Spell;
    [SerializeField] GameObject unit_BG;
    [SerializeField] GameObject[] buffUIEffects;
    [SerializeField] GameObject[] deBuffUIEffects;
    [Header("# BattleBanner")]
    [SerializeField] GameObject banner;
    [SerializeField] GameObject card_Box;


    [Header("# Object Size")]//대형 유닛 사이즈 조절
    [SerializeField] GameObject size_Obj;
    private void Awake()
    {
        UnitCollectionClear();
        SpellCollectionClear();
    }

    private void Update()
    {
        if (startSorangAnim)
        {
            sorangAnim = StartCoroutine(SorangDefenseAnim());
        }
        // 애니메이션 업데이트
        if (Time.timeScale == 0)
        {
            // 스켈레톤 애니메이션 수동 업데이트
            SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();
            unitSkeletonGraphic.Update(Time.unscaledDeltaTime);
            SkeletonGraphic unitSkeletonGraphic_Spell = unitGraphic_Spell.GetComponent<SkeletonGraphic>();
            unitSkeletonGraphic_Spell.Update(Time.unscaledDeltaTime);
        }
        }

        public void OpenUI(string UIName)
    {
        switch (UIName)
        {
            case "unitCollection":
                unitCollectionUI.SetActive(true);
                spellCollectionUI.SetActive(false);
                SpellCollectionClear();
                break;
            case "spellCollection":
                unitCollectionUI.SetActive(false);
                spellCollectionUI.SetActive(true);
                UnitCollectionClear();
                break;
        }
        if (card_Box != null)
        {
            Time.timeScale = 0;
            banner.transform.localScale = Vector3.zero;
            card_Box.transform.localScale = Vector3.zero;
        }
        // 사운드
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
    }

    public void CloseCollection()
    {
        UnitCollectionClear();
        SpellCollectionClear();
      
        if (card_Box != null)
        {
            banner.transform.localScale = Vector3.one;
            card_Box.transform.localScale = Vector3.one;
        }
        Time.timeScale = 1;
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

    void SpellCollectionClear()
    {
        spellCollectionUI.SetActive(false);
        spellNameText.text = "";
        spellCostText.text = "";
        spellPercentText.text = "";
        durationTimeText.text = "";
        spellEffectText.text = "";
        unitGraphic_Spell.SetActive(false);
        monster_Spell.SetActive(false);
        unit_BG.SetActive(false);

        unitGraphic_Spell.GetComponent<SkeletonGraphic>().startingAnimation = "Idle";

        // 모든 이펙트 종료
        SetUIEffect(buffUIEffects, -1);
        SetUIEffect(deBuffUIEffects, -1);
    }

    // 유닛 도감 버튼(유닛 카드) 함수
    public void GetUnitInfo(UnitData unitData)
    {
        if(unitData.UnitID >= 12003)//팬케이크, 거북
        {
            size_Obj.gameObject.transform.localScale = new Vector3(0.8f, 0.8f,1.3f);
        }
        else
        {
            size_Obj.gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }

        unitNameText.text = unitData.UnitName;
        unitHPText.text = unitData.Health.ToString();
        unitPowerText.text = unitData.Power.ToString();
        unitCostText.text = unitData.Cost.ToString();
        unitSpeedText.text = unitData.MoveSpeed.ToString();
        unitAtkSpeedText.text = unitData.AttackTime.ToString() + "s";
        usePercentText.text = GetUsePercentage(unitData.UnitID);
        animButtons.SetActive(true);

        if (sorangAnim != null) { StopCoroutine(sorangAnim); sorangAnim = null; startSorangAnim = false; Debug.Log("Strop SorangAnim"); }


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
        unitSkeletonGraphic.startingLoop = true;
        unitSkeletonGraphic.Initialize(true); // skeletonDataAsset 를 ReLoad
        unitGraphic.SetActive(true);

        if (unitData.UnitName == "빵게")
        {
            attackAnimText.text = "방어";
        }
        else
        {
            attackAnimText.text = "공격";
        }
    }

    // 스펠 도감 버튼(스펠 카드) 함수
    public void GetSpellInfo(SpellData spellData)
    {
        spellNameText.text = spellData.SpellName.ToString();
        spellCostText.text = spellData.Cost.ToString();
        spellPercentText.text = GetUsePercentage(spellData.SpellID);
        durationTimeText.text = spellData.Duration.ToString() + "s";
        spellEffectText.text = spellData.Spell_Effect;
        unit_BG.SetActive(true);

        if (spellData.SpellType == SpellData.SpellTypes.Buff)
        {
            unitGraphic_Spell.SetActive(true);
            monster_Spell.SetActive(false);

            SkeletonGraphic unitGraphic = unitGraphic_Spell.GetComponent<SkeletonGraphic>();

            switch (spellData.SpellID)
            {
                case 22002:
                case 22005:
                case 22006:
                    unitGraphic.startingAnimation = "Attack";
                    unitGraphic.Initialize(true);
                    break;
                case 22007:
                    unitGraphic.startingAnimation = "Walk";
                    unitGraphic.Initialize(true);
                    break;
                default:
                    unitGraphic.startingAnimation = "Idle";
                    unitGraphic.Initialize(true);
                    break;
            }
        }
        else if (spellData.SpellType == SpellData.SpellTypes.Debuff)
        {
            unitGraphic_Spell.SetActive(false);
            monster_Spell.SetActive(true);
        }


        // 스펠 UI 이펙트
        switch (spellData.SpellID)
        {
            case 22001:
                SetUIEffect(buffUIEffects, 0);
                break;
            case 22002:
                SetUIEffect(buffUIEffects, 1);
                break;
            case 22004:
                SetUIEffect(buffUIEffects, 2);
                break;
            case 22005:
                SetUIEffect(buffUIEffects, 3);
                break;
            case 22006:
                SetUIEffect(buffUIEffects, 4);
                break;
            case 22007:
                SetUIEffect(buffUIEffects, 5);
                break;
            case 23001:
                SetUIEffect(deBuffUIEffects, 0);
                break;
            case 23002:
                SetUIEffect(deBuffUIEffects, 1);
                break;
            case 23003:
                SetUIEffect(deBuffUIEffects, 2);
                break;
            case 23004:
                SetUIEffect(deBuffUIEffects, 3);
                break;

            case 23005:
                SetUIEffect(deBuffUIEffects, 4);
                break;

        }
    }

    // 스펠 이펙트 활성화/비활성화 함수
    void SetUIEffect(GameObject[] effets, int effectIndex)
    {
        for (int i = 0; i < effets.Length; i++)
        {
            effets[i].SetActive(i == effectIndex);
        }
    }

    // 캐릭터 도감 SkeletonGraphic 애니메이션 버튼
    public void UnitAnimButton(string animName)
    {
        if (sorangAnim != null) { StopCoroutine(sorangAnim); sorangAnim = null; startSorangAnim = false; Debug.Log("Strop SorangAnim"); }

        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();

        // 다른 애니메이션을 가진 unit 수정
        switch ((unitNameText.text, animName))
        {
            case ("빵게", "Attack"):
                // unitSkeletonGraphic.startingAnimation = "Defense_start";
                startSorangAnim = true;
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


        unitSkeletonGraphic.startingLoop = true;
        unitSkeletonGraphic.Initialize(true); // skeletonDataAsset 를 ReLoad
    }

    IEnumerator SorangDefenseAnim() // 수정 중
    {
        Debug.Log("SorangDefenseAnim Start");

        yield return null;
        startSorangAnim = false;

        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();

        unitSkeletonGraphic.startingAnimation = "Defense_start";
        unitSkeletonGraphic.Initialize(true);
        yield return new WaitForSeconds(0.9f);
        unitSkeletonGraphic.startingAnimation = "Defense_ing";
        unitSkeletonGraphic.Initialize(true);
        yield return new WaitForSeconds(0.9f);
        unitSkeletonGraphic.startingAnimation = "Defense_end";
        unitSkeletonGraphic.Initialize(true);
        yield return new WaitForSeconds(0.9f);

        if (sorangAnim != null) { StopCoroutine(sorangAnim); sorangAnim = null; startSorangAnim = true; }
    }

    // 카드 사용 퍼센트 출력 함수
    string GetUsePercentage(int id)
    {
        float allUseCount = 0; // 모든 카드의 사용 횟수
        float useCount = 0; // 선택한 카드의 사용 횟수
        float percent = 0;

        XmlNodeList allCardData = DBConnect.SelectOriginal("usingCount", "SELECT * FROM usingCount");

        for (int i = 0; i < allCardData.Count; i++)
        {
            allUseCount += int.Parse(allCardData[i].SelectSingleNode("count").InnerText);
        }

        XmlNodeList selectedCard = DBConnect.SelectOriginal("usingCount", $"SELECT count FROM usingCount WHERE cardID = {id};");

        if (selectedCard != null)
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
