using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaManager : MonoBehaviour
{
    public TMP_Text result_Text;

    // Temp
    public string result_Str = "Result = ";

    private int[] unitList_Common = { 1 };
    private int[] unitList_Rare = { 2 };
    private int[] unitList_Epic = { 3 };

    [SerializeField] private List<int> result = new List<int>();

    public void Gacha_Btn(int number)
    {
        //팝업 출력 등 활용 가능 메소드
        Gacha_Rarity(number);
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
        }
        Gacha_Result(result.Count);
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
}