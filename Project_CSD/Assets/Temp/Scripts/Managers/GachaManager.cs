using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class GachaManager : MonoBehaviour
{
    public static GachaManager single;
    public GameObject cashMgr;
    public TMP_Text result_Text;
    public GameObject gacha_Result;

    // Temp
    public string result_Str = "Result = ";

    private int[] unitList_Common = { 1 };
    private int[] unitList_Rare = { 2 };
    private int[] unitList_Epic = { 3 };

    [SerializeField] private List<int> result = new List<int>();
    // ĳ�� �Ŵ��� ��ũ��Ʈ ���� ����
    private CashManager cashManagerScript;
    [SerializeField] HoldingList holdingScript;

    [Header("Gacha_Item_Info")]//��í ������ ����
    
    [SerializeField] private GameObject gacha_Item;
    [SerializeField] private Transform gacha_Tr;

    [Header("��í ���ø�")]
    public List<UnitData> listGachaTemplete;
    public List<SpellData> listSpellItem;
    public void Awake()
    {
        single = this;
        if (cashMgr == null)
        {
            cashMgr = GameObject.Find("CashManager");
        }
        // CashManager ��ũ��Ʈ�� �����ɴϴ�.
        if (cashMgr != null)
        {
            cashManagerScript = cashMgr.GetComponent<CashManager>();
        }
    }
    public void Gacha_Btn(int number)
    {

        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }

        if (cashManagerScript.player_Cash >= 1 && number == 1)
        {

            Item_Destroy();
            cashManagerScript.player_Cash = cashManagerScript.player_Cash - 1;
            Gacha_Rarity(number);
            gacha_Result.SetActive(true);


        }
        else if (cashManagerScript.player_Cash >= 10 && number == 10)
        {
            Item_Destroy();
            cashManagerScript.player_Cash = cashManagerScript.player_Cash - 10;
            Gacha_Rarity(number);
            gacha_Result.SetActive(true);
        }
        //�˾� ��� �� Ȱ�� ���� �޼ҵ�
    }

    void Gacha_Rarity(int number)
    {
        result.Clear();
        result_Text.text = "";
        result_Str = "";

        StartCoroutine(GachaProcess(number));
    }

    IEnumerator GachaProcess(int number)
    {
        for (int i = 1; i <= number; i++)
        {
            ItemInit();
            yield return new WaitForSeconds(2f);
        }
        // Gacha_Result(result.Count);
    }

    void ItemInit()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(1, 100);
        if (randomValue <= 24) // Rare 21%
        {
            UnitData dataRandom = listGachaTemplete[Random.Range(0, listGachaTemplete.Count)];
            holdingScript.Update_Hoding(dataRandom);

            GameObject temp = Instantiate(gacha_Item, gacha_Tr.position, Quaternion.identity);
            temp.transform.SetParent(gacha_Tr.transform);

            Gacha item = temp.GetComponent<Gacha>();
            item.Init(dataRandom);

        }
        else // Common 76%
        {

            SpellData dataRandom = listSpellItem[Random.Range(0, listSpellItem.Count)];
            holdingScript.Update_SpellHoding(dataRandom);

            GameObject temp = Instantiate(gacha_Item, gacha_Tr.position, Quaternion.identity);
            temp.transform.SetParent(gacha_Tr.transform);

            Gacha item = temp.GetComponent<Gacha>();
            item.SpellInit(dataRandom);
            //Gacha_Unit(unitList_Common, "Common");
        }
    }

    void Gacha_Unit(int[] unitList, string rarity)
    {
        int[] pick = unitList;

        System.Random random = new System.Random();
        int randomValue = random.Next(0, pick.Length);

        result.Add(pick[randomValue]);

        // rarity : UI Color Change
    }

    void Gacha_Result(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i == (count - 1))
            {
                result_Str += result[i];
            }
            else
            {
                result_Str += result[i] + ", ";
            }
        }
        result_Text.text = result_Str;
    }

    public void result_Off()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        gacha_Result.SetActive(false);
    }

    public void Item_Destroy()
    {

        foreach (Transform child in gacha_Tr)
        {
            Destroy(child.gameObject);
        }
    }
}