using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System; 
public class BreakRackMgr : MonoBehaviour
{
    public static BreakRackMgr Instance;

    [Header("BreadRack 아이템 등급")]
    [SerializeField] private GameObject legendary_Icon;
    [SerializeField] private GameObject legendary_Cost;

    [Header("Bread오브젝트 정보")]
    public TextMeshProUGUI breadRack_Name;
    public TextMeshProUGUI breadRack_No;
    public TextMeshProUGUI breadRack_Cost;
    public TextMeshProUGUI breadRack_Level;
    public Image breadRack_Img;
    public float breadRack_Stats;
    public TextMeshProUGUI breadRack_InfoTxt;

    [Header("BreadRack ShowCase")]
    public Sprite show_Default;
    public GameObject[] set_Item = new GameObject[8];
    
    public Transform set_Top;
    public Transform set_Bottom;
    public int set_item_Value = 0;


    [Header("BreadRack ShowCase Status")]//최종 스테이터스
    public TextMeshProUGUI attack_Status;
    public TextMeshProUGUI speed_Status;
    public TextMeshProUGUI mainHP_Status;
    public TextMeshProUGUI unitHP_Status;
    public float atk_Stu=0;
    public float speed_Stu=0;
    public float mainHp_Stu=0;
    public float unitHp_Stu=0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
    
    private void Start()
    {
        attack_Status.text = "Attack    ";
        speed_Status.text = "Speed  ";
        mainHP_Status.text = "MainHP    ";
        unitHP_Status.text = "UnitHP    ";
    }
    private void Update()
    {
       
      
    }
    public void SetItem(BreadRack_Data data)
    {
        int value_No = 0;
        for (int i = set_Item.Length-1; i >= 0; i--)
        {
            Show_Case_Item show = set_Item[i].GetComponent<Show_Case_Item>();
            if (show.setting == false)
            {
                value_No = i;
            }
        }

        Image item_Img = set_Item[value_No].GetComponent<Image>();
        Show_Case_Item show_Item = set_Item[value_No].GetComponent<Show_Case_Item>();

        item_Img.sprite = data.BreadRack_Img;
        show_Item.show_Value = set_item_Value;
        show_Item.item_No = data.BreadRack_No;
        show_Item.setting = true;
        set_item_Value++;

        ShowCase_Status(data);
    }

    public void Cancel_SetItem(BreadRack_Data data)
    {
        for(int i=0; i < set_Item.Length; i++)
        {
            Show_Case_Item show_Item = set_Item[i].GetComponent<Show_Case_Item>();
            if(show_Item.item_No == data.BreadRack_No)
            {
              
                Image item_Img = set_Item[i].GetComponent<Image>();
                item_Img.sprite = show_Default;
                show_Item.setting = false;
                set_item_Value -= 1;
                ShowCase_Status_Cancel(data);
            }
        }


    }
    public void SaveStatus()
    {
        if (PlayerData.instance != null )
        {
            PlayerData.instance.atk_Stu = atk_Stu;
            PlayerData.instance.speed_Stu = speed_Stu;
            PlayerData.instance.mainHp_Stu = mainHp_Stu;
            PlayerData.instance.unitHp_Stu = unitHp_Stu;
        }
    }

    public void ShowCase_Status(BreadRack_Data data)
    {
        
        float type = data.BreadRack_No / (int)Math.Pow(10, (int)Math.Log10(data.BreadRack_No)); ;
        if(type == 1)
        {
            atk_Stu += data.BreadRack_Atk;
            attack_Status.text = "Attack    " + atk_Stu + "X";
        
          
        }
        else if(type == 2)
        {
            speed_Stu += data.BreadRack_Spd;
            speed_Status.text = "Speed  " + speed_Stu + "X";
          
        }
        else if (type == 3)
        {
            mainHp_Stu += data.BreadRack_MainHp;
            mainHP_Status.text = "MainHP    " + mainHp_Stu + "X";
    
        }
        else if (type == 4)
        {
            unitHp_Stu += data.BreadRack_UnitHp;
            unitHP_Status.text = "UnitHP    " + unitHp_Stu + "X";
       
        }
        
    }
    public void ShowCase_Status_Cancel(BreadRack_Data data)
    {

        float type = data.BreadRack_No / (int)Math.Pow(10, (int)Math.Log10(data.BreadRack_No)); ;
        if (type == 1)
        {
            atk_Stu -= data.BreadRack_Atk;
            attack_Status.text = "Attack    " + atk_Stu + "X";
        
        }
        else if (type == 2)
        {
            speed_Stu -= data.BreadRack_Spd;
            speed_Status.text = "Speed  " + speed_Stu + "X";
           
        }
        else if (type == 3)
        {
            mainHp_Stu -= data.BreadRack_MainHp;
            mainHP_Status.text = "MainHP    " + mainHp_Stu + "X";
           
        }
        else if (type == 4)
        {
            unitHp_Stu -= data.BreadRack_UnitHp;
            unitHP_Status.text = "UnitHP    " + unitHp_Stu + "X";
           
        }

    }

   
    public void BreadInfoValue(BreadRack_Data data)//아이템 정보 표시
    {


        int type = data.BreadRack_RareType;
        if(type == 1)//아이템 정보창 등급 표시
        {
            legendary_Cost.SetActive(false);
            legendary_Icon.SetActive(false);
           
        }
        else
        {
            legendary_Cost.SetActive(true);
            legendary_Icon.SetActive(true);
        }
        breadRack_Name.text = data.BreadRack_Name;
        breadRack_Cost.text = data.BreadRack_Cost.ToString();
        breadRack_Level.text = "Lv " + data.BreadRack_Level.ToString();
        breadRack_Img.sprite = data.BreadRack_Img;
        breadRack_InfoTxt.text = data.BreadRack_InfoTxt;

        SetItem(data);
    }


}
