using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DBConnect : Singleton<DBConnect>
{
    void Awake()
    {
        Debug.Log("Connection Test :" + Connection());
    }

    private void Start()
    {
        // Insert("ranking", 2001565, "ȫ�浿", 10);
        // ������ �˻� �� ������ Insert ������ Update
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            XmlNodeList selectedData = Select("ranking", $"WHERE id = {2001565}");

            if (selectedData == null)
            {
                Insert("ranking", 2001565, "ȫ�浿", 10);
            }
            else
            {
                Update("ranking", "name", "time", "¯��", 5, "id = 2001565");
            }
        }
    }


    // MySql ���� �Լ�
    public bool Connection()
    {
        string conStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode=none;",
         "34.64.201.214", "cst", "root", "yju123", "3306");
        //"34.64.201.214", "cst", "ori", "Asdf1478!", "3306");
        //"127.0.0.1", "testDB", "root", "1478", "3306");    // -> �� ���� db

        try
        {
            Debug.Log("Attempting to open connection..." + conStr);
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

    private static MySqlConnection _connection = null;
    private static MySqlConnection connection // ȣ�� �� ����Ǵ� ����
    {
        get
        {
            if (_connection == null)
            {
                try
                {
                    string formatSql = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode=none;",
                                                        "34.64.201.214", "cst", "root", "yju123", "3306");
                    _connection = new MySqlConnection(formatSql);
                }
                catch (MySqlException e)
                {
                    Debug.LogError(e);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            return _connection;
        }
    }

    // MySql ������ ���� �Լ�
    private static bool m_OnChange(string query)
    {
        bool result = false;
        try
        {
            MySqlCommand sqlCommand = new MySqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandText = query;

            connection.Open();

            sqlCommand.ExecuteNonQuery();

            connection.Close();

            result = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        connection.Close();
        return result;
    }

    // �����ͺ��̽����� �����͸� �������� �Լ�
    private static DataSet m_OnLoad(string tableName, string query)
    {
        DataSet ds = null; ;
        try
        {
            connection.Open();   //DB ����

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = query;

            MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            sd.Fill(ds, tableName);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        connection.Close();  //DB ���� ����
        return ds;
    }

    /// <summary>
    /// ������ �˻�
    /// </summary>
    /// <param name="tableName">�˻��� ���̺�</param>
    /// <param name="field">�˻��� �ʵ� (�Է����� ���� ��� ��ü �ε�)</param>
    /// <param name="condition">����</param>
    /// <returns></returns>
    public static XmlNodeList Select(string tableName, string condition = "")
    {
        DataSet dataSet = m_OnLoad(tableName, $"SELECT * FROM {tableName} {condition}");

        if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            return null;

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());

        return xmlDocument.GetElementsByTagName(tableName);
    }

    /// <summary>
    /// ������ �Է�
    /// </summary>
    /// <param name="tableName">�Է��� ���̺�</param>
    /// <param name="fieldName">�Է��� �ʵ� �̸�</param>
    /// <param name="value">�Է��� ��</param>
    /// <returns></returns>
    public static bool Insert(string tableName, string fieldName, string value)
    {
        return m_OnChange($"INSERT INTO {tableName} ({fieldName}) VALUES ('{value}')");
    }

    /// <summary>
    /// ������ �Է� (���������� ���� �Էµ�)
    /// </summary>
    /// <param name="tableName">�Է��� ���̺�</param>
    /// <param name="values">�Է��� �� (Į�� ������� �����)</param>
    /// <returns></returns>
    public static bool Insert(string tableName, int id, string name, int time)
    {
        string strValues = string.Empty;

        strValues = $"{id}, '{name}', {time}";

        Debug.Log("Insert Data");
        return m_OnChange($"INSERT INTO {tableName} VALUES ({strValues})");
    }

    /// <summary>
    /// ���ڵ� ����
    /// </summary>
    /// <param name="tableName">�Է��� ���̺�</param>
    /// <param name="fieldName">�Է��� �ʵ� �̸�</param>
    /// <param name="value">�Է��� ��</param>
    /// <param name="condition">����</param>
    /// <returns></returns>
    public static bool Update(string tableName, string fieldName1, string fieldName2, string name, int time, string condition)
    {
        Debug.Log("Update Data");
        return m_OnChange($"UPDATE {tableName} SET {fieldName1}='{name}', {fieldName2}={time} WHERE {condition}");
    }


    /// <summary>
    /// ���ڵ� ����
    /// </summary>
    /// <param name="tableName">������ ���ڵ尡 ���Ե� ���̺�</param>
    /// <param name="condition">����</param>
    /// <returns></returns>
    public static bool Delete(string tableName, string condition)
    {
        return m_OnChange($"DELETE FROM {tableName} WHERE {condition}");
    }
}

