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
}