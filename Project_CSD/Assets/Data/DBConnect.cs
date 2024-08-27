using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class DBConnect : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Connection Test :" + Connection());
    }


    public bool Connection()
    {
        string conStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};",
                                  "34.64.201.214", "cst", "root", "yju123");

        try
        {
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                con.Open();
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("e : " + e.ToString());
            return false;
        }
    }
}
