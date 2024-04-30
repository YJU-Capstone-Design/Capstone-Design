using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBTest : MonoBehaviour
{
    public PlayerInfoDB testdb;
    private void Start()
    {
        Debug.Log("no : " + testdb.sheet1[0].Player_No + " name : " + testdb.sheet1[0].NAME);
        Debug.Log("id : " + testdb.sheet1[0].ID + " pwd : " + testdb.sheet1[0].PASS);
        Debug.Log("lv : " + testdb.sheet1[0].Lv + " char : " + testdb.sheet1[0].Char + "spell : " + testdb.sheet1[0].Spell);
        testdb.sheet1[0].Player_No = 1233;
        Debug.Log("no : " + testdb.sheet1[0].Player_No);
        testdb.sheet1[1].Player_No = 1234;
        Debug.Log("no : " + testdb.sheet1[1].Player_No);
    }
}
