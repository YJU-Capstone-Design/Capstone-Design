using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public int Player_No;
    public string NAME;
    public string ID;
    public string PASS;
    public int Lv;
    public int Char;
    public int Spell;
    public string cash;
    public string gold;
    public Sprite icon;
    public List<Sprite> addicon = new List<Sprite>();

    [Header("BreakRack")]
    public float atk_Stu =1;
    public float speed_Stu = 1;
    public float mainHp_Stu = 1;
    public float unitHp_Stu = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMember(string name, string id, string pw, Sprite sprite)
    {
        Player_No = 80000001;
        NAME = name;
        ID = id;
        PASS = pw;
        icon = sprite;
        Lv = 1;
    }

    public void AddIcon(Sprite newIcon)
    {
        // 중복 추가 방지
        if (!addicon.Contains(newIcon))
        {
            addicon.Add(newIcon);
        }
    }
    
}
