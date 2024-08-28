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
        string conStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode=none;",
            "34.64.201.214", "cst", "root", "yju123", "3306");
        //"34.64.201.214", "cst", "ori", "Asdf1478!", "3306");
        //"127.0.0.1", "testDB", "root", "1478", "3306");    // -> ³» ·ÎÄÃ db

        try
        {
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                con.Open();
            }
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.LogError("MySQL Error: " + ex.Message);
            if (ex.InnerException != null)
            {
                Debug.LogError("Inner Exception: " + ex.InnerException.Message);
            }
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError("General Error: " + ex.Message);
            if (ex.InnerException != null)
            {
                Debug.LogError("Inner Exception: " + ex.InnerException.Message);
            }
            return false;
        }
    }
}
