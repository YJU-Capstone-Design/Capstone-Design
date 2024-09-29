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


    private void Awake()
    {
        UnitCollectionClear();
        SpellCollectionClear();
    }

    //private void Update() ���� ��
    //{
    //    if (startSorangAnim && sorangAnim == null)
    //    {
    //        sorangAnim = StartCoroutine(SorangDefenseAnim());
    //    }
    //    if (sorangAnim != null) { Debug.Log("Start Anim"); }
    //}

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

        // ����
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
    }

    public void CloseCollection()
    {
        UnitCollectionClear();
        SpellCollectionClear();

        // ����
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
        attackAnimText.text = "����";
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

        // ��� ����Ʈ ����
        SetUIEffect(buffUIEffects, -1);
        SetUIEffect(deBuffUIEffects, -1);
    }

    // ���� ���� ��ư(���� ī��) �Լ�
    public void GetUnitInfo(UnitData unitData)
    {
        unitNameText.text = unitData.UnitName;
        unitHPText.text = unitData.Health.ToString();
        unitPowerText.text = unitData.Power.ToString();
        unitCostText.text = unitData.Cost.ToString();
        unitSpeedText.text = unitData.MoveSpeed.ToString();
        unitAtkSpeedText.text = unitData.AttackTime.ToString() + "s";
        usePercentText.text = GetUsePercentage(unitData.UnitID);
        animButtons.SetActive(true);

        // ���� Spine UI
        unitGraphic.SetActive(false);
        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();
        unitSkeletonGraphic.skeletonDataAsset = unitData.Unit_skeletonData;
        if (unitData.UnitName == "���׺�")
        {
            unitSkeletonGraphic.startingAnimation = "idle_3unit";
        }
        else
        {
            unitSkeletonGraphic.startingAnimation = "Idle";
        }
        unitSkeletonGraphic.startingLoop = true;
        unitSkeletonGraphic.Initialize(true); // skeletonDataAsset �� ReLoad
        unitGraphic.SetActive(true);

        if(unitData.UnitName == "����")
        {
            attackAnimText.text = "���";
        }
        else
        {
            attackAnimText.text = "����";
        }
    }

    // ���� ���� ��ư(���� ī��) �Լ�
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
        else if(spellData.SpellType == SpellData.SpellTypes.Debuff)
        {
            unitGraphic_Spell.SetActive(false);
            monster_Spell.SetActive(true);
        }


        // ���� UI ����Ʈ
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

    // ���� ����Ʈ Ȱ��ȭ/��Ȱ��ȭ �Լ�
    void SetUIEffect(GameObject[] effets,int effectIndex)
    {
        for (int i = 0; i < effets.Length; i++)
        {
            effets[i].SetActive(i == effectIndex);
        }
    }

    // ĳ���� ���� SkeletonGraphic �ִϸ��̼� ��ư
    public void UnitAnimButton(string animName)
    {
        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();

        // �ٸ� �ִϸ��̼��� ���� unit ����
        switch ((unitNameText.text, animName))
        {
            case ("����", "Attack"):
                unitSkeletonGraphic.startingAnimation = "Defense_start";
                break;
            case ("���׺�", "Idle"):
                unitSkeletonGraphic.startingAnimation = "idle_3unit";
                break;
            case ("���׺�", "Walk"):
                unitSkeletonGraphic.startingAnimation = "walk_3unit";
                break;
            case ("���׺�", "Attack"):
                unitSkeletonGraphic.startingAnimation = "attack_3unit";
                break;
            case ("���׺�", "Win"):
                unitSkeletonGraphic.startingAnimation = "win_3unit";
                break;
            case ("���׺�", "Die"):
                unitSkeletonGraphic.startingAnimation = "die_3unit";
                break;
            default:
                unitSkeletonGraphic.startingAnimation = animName;
                break;
        }


        unitSkeletonGraphic.startingLoop = true;
        unitSkeletonGraphic.Initialize(true); // skeletonDataAsset �� ReLoad
    }

    IEnumerator SorangDefenseAnim() // ���� ��
    {
        Debug.Log("SorangDefenseAnim Start");
        startSorangAnim = false;

        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();

        unitSkeletonGraphic.startingAnimation = "Defense_start";
        unitSkeletonGraphic.Initialize(true);
        yield return new WaitForSeconds(1);
        unitSkeletonGraphic.startingAnimation = "Defense_ing";
        unitSkeletonGraphic.Initialize(true);
        yield return new WaitForSeconds(1);
        unitSkeletonGraphic.startingAnimation = "Defense_end";
        unitSkeletonGraphic.Initialize(true);
        yield return new WaitForSeconds(1);

        sorangAnim = null;
        startSorangAnim = true;
    }

    // ī�� ��� �ۼ�Ʈ ��� �Լ�
    string GetUsePercentage(int id)
    {
        float allUseCount = 0; // ��� ī���� ��� Ƚ��
        float useCount = 0; // ������ ī���� ��� Ƚ��
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
        percent = (float)Math.Round(percent, 2); // �Ҽ��� 2�ڸ� �ݿø�

        Debug.Log($"useCount : {useCount}, allUseCount : {allUseCount}, percent : {percent}");

        // �ؽ�Ʈ return
        return $"{percent}%";
    }
}
