using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;

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
    // 캐시 매니저 스크립트 참조 변수
    private CashManager cashManagerScript;
    [SerializeField] HoldingList holdingScript;

    [Header("Gacha_Item_Info")] // 가챠 아이템 정보
    [SerializeField] private GameObject gacha_Item;
    [SerializeField] private Transform gacha_Tr;

    [Header("가챠 템플릿")]
    public List<UnitData> listGachaTemplete;
    public List<SpellData> listSpellItem;

    private Coroutine gachaCoroutine;

    public void Awake()
    {
        single = this;
        if (cashMgr == null)
        {
            cashMgr = GameObject.Find("CashManager");
        }
        // CashManager 스크립트를 가져옵니다.
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
            cashManagerScript.player_Cash -= 1;
            Gacha_Rarity(number);
            gacha_Result.SetActive(true);
        }
        else if (cashManagerScript.player_Cash >= 10 && number == 10)
        {
            Item_Destroy();
            cashManagerScript.player_Cash -= 10;
            Gacha_Rarity(number);
            gacha_Result.SetActive(true);
        }
        // 팝업 출력 등 활용 가능 메소드
    }

    void Gacha_Rarity(int number)
    {
        result.Clear();
        result_Text.text = "";
        result_Str = "";

        gacha = number;
        if (gachaCoroutine != null)
        {
            StopCoroutine(gachaCoroutine);
        }
        gachaCoroutine = StartCoroutine(GachaProcess(number));
    }

    public int gacha_Value;
    public int gacha;

    IEnumerator GachaProcess(int number)
    {
        for (gacha_Value = 0; gacha_Value < number; gacha_Value++)
        {
            ItemInit();
            yield return new WaitForSeconds(2f);
        }
        // Gacha_Result(result.Count);
    }

    public void Skip()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        if (gachaCoroutine != null)
        {
            StopCoroutine(gachaCoroutine);
        }
        Debug.Log(gacha_Value + " : " + gacha);
        for (int i = gacha_Value+1; i < gacha; i++)
        {
            ItemInit();
        }
        gacha_Value = 0;
        gacha = 0;
    }

    public void result_Off()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        if (gachaCoroutine != null)
        {
            StopCoroutine(gachaCoroutine);
        }
        Debug.Log(gacha_Value + " : " + gacha);
        for (int i = gacha_Value+1; i < gacha; i++)
        {
            ItemInit();
        }
        gacha_Value = 0;
        gacha = 0;
        gacha_Result.SetActive(false);
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
            temp.transform.SetParent(gacha_Tr);

            Gacha item = temp.GetComponent<Gacha>();
            item.Init(dataRandom);
        }
        else // Common 76%
        {
            SpellData dataRandom = listSpellItem[Random.Range(0, listSpellItem.Count)];
            holdingScript.Update_SpellHoding(dataRandom);

            GameObject temp = Instantiate(gacha_Item, gacha_Tr.position, Quaternion.identity);
            temp.transform.SetParent(gacha_Tr);

            Gacha item = temp.GetComponent<Gacha>();
            item.SpellInit(dataRandom);
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

    public void Item_Destroy()
    {
        foreach (Transform child in gacha_Tr)
        {
            Destroy(child.gameObject);
        }
    }
}
