using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Info : Singleton<Player_Info>
{
    [Header("player_info")]
    public int player_No;
    public string player_Name;
    public string player_ID;
    public string player_Pwd;
    public int player_Lv;
    public int player_Have_Char;
    public int player_Have_Spell;
    public int player_Gold = 0;
    public int player_Cash = 0;
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI CashText;
    public void Awake()
    {
        GoldText.text = player_Gold.ToString();
        CashText.text = player_Cash.ToString();
    }
}