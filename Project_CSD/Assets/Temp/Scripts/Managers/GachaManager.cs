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
    // ĳ�� �Ŵ��� ��ũ��Ʈ ���� ����
    private CashManager cashManagerScript;


    [Header("Gacha_Item_Info")]//��í ������ ����
    
    [SerializeField] private GameObject gacha_Item;
    [SerializeField] private Transform gacha_Tr;
    public void Awake()
    {
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
        if(cashManagerScript.player_Cash >= 1 && number == 1)
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

        for (int i = 1; i <= number; i++)
        {
            GameObject temp = Instantiate(gacha_Item, gacha_Tr.position, Quaternion.identity);
            temp.transform.SetParent(gacha_Tr.transform);
            /*System.Random random = new System.Random();
            int randomValue = random.Next(1, 100);

            if (randomValue <= 3) // Epic 3%
            {
                //Gacha_Unit(unitList_Epic, "Epic");
               
                    GameObject temp = Instantiate(gacha_Item, gacha_Tr.position, Quaternion.identity);
                    temp.transform.SetParent(this.transform);
                
                   
                
            }
            else if (randomValue <= 24) // Rare 21%
            {
                GameObject temp = Instantiate(gacha_Item, gacha_Tr.position, Quaternion.identity);
                temp.transform.SetParent(this.transform);

           
                //Gacha_Unit(unitList_Rare, "Rare");
            }
            else // Common 76%
            {
                GameObject temp = Instantiate(gacha_Item, gacha_Tr.position, Quaternion.identity);
                temp.transform.SetParent(gacha_Tr.transform);

               
                //Gacha_Unit(unitList_Common, "Common");
            }*/
        }
       // Gacha_Result(result.Count);
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

    public void Item_Destroy()
    {

        foreach (Transform child in gacha_Tr)
        {
            Destroy(child.gameObject);
        }
    }
}