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
    Coroutine defenseAnim;
    Coroutine attackAnim;
    [SerializeField] bool startDefenseAnim = false;
    [SerializeField] bool startAttackAnim = false;

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


    [Header("# Object Size")]//���� ���� ������ ����
    [SerializeField] GameObject size_Obj;
    private void Awake()
    {
        UnitCollectionClear();
        SpellCollectionClear();
    }

    private void Update()
    {
        if (startDefenseAnim)
        {
            defenseAnim = StartCoroutine(TankerDefenseAnim());
        }

        if (startAttackAnim)
        {
            attackAnim = StartCoroutine(AttackAnim());
        }

        // �ִϸ��̼� ������Ʈ
        if (Time.timeScale == 0)
        {
            // ���̷��� �ִϸ��̼� ���� ������Ʈ
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
        // ����
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
        if(unitData.UnitID == 12003|| unitData.UnitID == 11006)//������ũ, �ź�
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

        if (defenseAnim != null) { StopCoroutine(defenseAnim); defenseAnim = null; startDefenseAnim = false; Debug.Log("Strop DefenseAnim"); }
        if (attackAnim != null) { StopCoroutine(attackAnim); attackAnim = null; startAttackAnim = false; Debug.Log("Stop AttackAnim"); }


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

        if (unitData.UnitName == "����" || unitData.UnitName == "������ũ")
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
        else if (spellData.SpellType == SpellData.SpellTypes.Debuff)
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
    void SetUIEffect(GameObject[] effets, int effectIndex)
    {
        for (int i = 0; i < effets.Length; i++)
        {
            effets[i].SetActive(i == effectIndex);
        }
    }

    // ĳ���� ���� SkeletonGraphic �ִϸ��̼� ��ư
    public void UnitAnimButton(string animName)
    {
        if (defenseAnim != null) { StopCoroutine(defenseAnim); defenseAnim = null; startDefenseAnim = false; Debug.Log("Stop DefenseAnim"); }
        if (attackAnim != null) { StopCoroutine(attackAnim); attackAnim = null; startAttackAnim = false; Debug.Log("Stop AttackAnim"); }

        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();

        // �ٸ� �ִϸ��̼��� ���� unit ����
        switch ((unitNameText.text, animName))
        {
            case ("����", "Attack"):
            case ("������ũ", "Attack"):
                startDefenseAnim = true;
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
            case ("�źϷл�", "Attack"):
                startAttackAnim = true;
                break;
            default:
                unitSkeletonGraphic.startingAnimation = animName;
                break;
        }


        unitSkeletonGraphic.startingLoop = true;
        unitSkeletonGraphic.Initialize(true); // skeletonDataAsset �� ReLoad
    }

    IEnumerator TankerDefenseAnim()
    {
        Debug.Log("TankerDefenseAnim Start");

        startDefenseAnim = false;

        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();

        unitSkeletonGraphic.startingAnimation = "Defense_start";
        unitSkeletonGraphic.Initialize(true);

        if(unitNameText.text == "����") { yield return new WaitForSeconds(0.9f); }
        else if(unitNameText.text == "������ũ") { yield return new WaitForSeconds(1.9f); }

        unitSkeletonGraphic.startingAnimation = "Defense_ing";
        unitSkeletonGraphic.Initialize(true);

        yield return new WaitForSeconds(0.9f);

        unitSkeletonGraphic.startingAnimation = "Defense_end";
        unitSkeletonGraphic.Initialize(true);

        if (unitNameText.text == "����") { yield return new WaitForSeconds(0.9f); }
        else if (unitNameText.text == "������ũ") { yield return new WaitForSeconds(1.9f); }

        if (defenseAnim != null) { StopCoroutine(defenseAnim); defenseAnim = null; startDefenseAnim = true; }
    }

    IEnumerator AttackAnim()
    {
        Debug.Log("AttackAnim Start");

        startAttackAnim = false;

        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();

        unitSkeletonGraphic.startingAnimation = "Attack_start";
        unitSkeletonGraphic.Initialize(true);

        yield return new WaitForSeconds(1.9f);

        unitSkeletonGraphic.startingAnimation = "Attack_ing";
        unitSkeletonGraphic.Initialize(true);

        yield return new WaitForSeconds(0.35f);

        unitSkeletonGraphic.startingAnimation = "Attack_end";
        unitSkeletonGraphic.Initialize(true);

        yield return new WaitForSeconds(3.9f);

        if (attackAnim != null) { StopCoroutine(attackAnim); attackAnim = null; startAttackAnim = true; }
    }

    // ī�� ��� �ۼ�Ʈ ��� �Լ�
    string GetUsePercentage(int id)
    {
        float allUseCount = 0; // ��� ī���� ��� Ƚ��
        float useCount = 0; // ������ ī���� ��� Ƚ��
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
        percent = (float)Math.Round(percent, 2); // �Ҽ��� 2�ڸ� �ݿø�

        Debug.Log($"useCount : {useCount}, allUseCount : {allUseCount}, percent : {percent}");

        // �ؽ�Ʈ return
        return $"{percent}%";
    }
}
