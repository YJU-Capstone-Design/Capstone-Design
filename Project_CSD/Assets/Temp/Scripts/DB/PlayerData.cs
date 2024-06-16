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

}
