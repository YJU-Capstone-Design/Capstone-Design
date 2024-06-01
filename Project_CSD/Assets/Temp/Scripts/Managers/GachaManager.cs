using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class GachaManager : MonoBehaviour
{
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

    public void Awake()
    {
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
        if(cashManagerScript.player_Cash >= 1 && number == 1)
        {
            cashManagerScript.player_Cash = cashManagerScript.player_Cash - 1;
            Gacha_Rarity(number);
            gacha_Result.SetActive(true);
        }
        else if (cashManagerScript.player_Cash >= 10 && number == 10)
        {
            cashManagerScript.player_Cash = cashManagerScript.player_Cash - 10;
            Gacha_Rarity(number);
            gacha_Result.SetActive(true);
        }
        //팝업 출력 등 활용 가능 메소드
    }

    void Gacha_Rarity(int number)
    {
        result.Clear();
        result_Text.text = "";

        result_Str = "";

        for (int i = 1; i <= number; i++)
            {
                System.Random random = new System.Random();
                int randomValue = random.Next(1, 100);

                if (randomValue <= 3) // Epic 3%
                {
                    Gacha_Unit(unitList_Epic, "Epic");
                }
                else if (randomValue <= 24) // Rare 21%
                {
                    Gacha_Unit(unitList_Rare, "Rare");
                }
                else // Common 76%
                {
                    Gacha_Unit(unitList_Common, "Common");
                }

                Gacha_Result(result.Count);
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
        gacha_Result.SetActive(false);
    }
}