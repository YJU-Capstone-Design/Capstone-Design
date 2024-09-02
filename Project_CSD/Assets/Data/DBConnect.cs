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
        // Insert("ranking", 2001565, "홍길동", 10);
        // 데이터 검색 후 없으면 Insert 있으면 Update
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            XmlNodeList selectedData = Select("ranking", $"WHERE id = {2001565}");

            if (selectedData == null)
            {
                Insert("ranking", 2001565, "홍길동", 10);
            }
            else
            {
                Update("ranking", "name", "time", "짱구", 5, "id = 2001565");
            }
        }
    }


    // MySql 연결 함수
    public bool Connection()
    {
        string conStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode=none;",
         "34.64.201.214", "cst", "root", "yju123", "3306");
        //"34.64.201.214", "cst", "ori", "Asdf1478!", "3306");
        //"127.0.0.1", "testDB", "root", "1478", "3306");    // -> 내 로컬 db

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
    private static MySqlConnection connection // 호출 시 실행되는 구조
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

    // MySql 쿼리문 실행 함수
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

    // 데이터베이스에서 데이터를 가져오는 함수
    private static DataSet m_OnLoad(string tableName, string query)
    {
        DataSet ds = null; ;
        try
        {
            connection.Open();   //DB 연결

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

        connection.Close();  //DB 연결 해제
        return ds;
    }

    /// <summary>
    /// 데이터 검색
    /// </summary>
    /// <param name="tableName">검색할 테이블</param>
    /// <param name="field">검색할 필드 (입력하지 않을 경우 전체 로드)</param>
    /// <param name="condition">조건</param>
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
    /// 데이터 입력
    /// </summary>
    /// <param name="tableName">입력할 테이블</param>
    /// <param name="fieldName">입력할 필드 이름</param>
    /// <param name="value">입력할 값</param>
    /// <returns></returns>
    public static bool Insert(string tableName, string fieldName, string value)
    {
        return m_OnChange($"INSERT INTO {tableName} ({fieldName}) VALUES ('{value}')");
    }

    /// <summary>
    /// 데이터 입력 (순차적으로 전부 입력됨)
    /// </summary>
    /// <param name="tableName">입력할 테이블</param>
    /// <param name="values">입력할 값 (칼럼 순서대로 적용됨)</param>
    /// <returns></returns>
    public static bool Insert(string tableName, int id, string name, int time)
    {
        string strValues = string.Empty;

        strValues = $"{id}, '{name}', {time}";

        Debug.Log("Insert Data");
        return m_OnChange($"INSERT INTO {tableName} VALUES ({strValues})");
    }

    /// <summary>
    /// 레코드 갱신
    /// </summary>
    /// <param name="tableName">입력할 테이블</param>
    /// <param name="fieldName">입력할 필드 이름</param>
    /// <param name="value">입력할 값</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static bool Update(string tableName, string fieldName1, string fieldName2, string name, int time, string condition)
    {
        Debug.Log("Update Data");
        return m_OnChange($"UPDATE {tableName} SET {fieldName1}='{name}', {fieldName2}={time} WHERE {condition}");
    }


    /// <summary>
    /// 레코드 제거
    /// </summary>
    /// <param name="tableName">제거할 레코드가 포함된 테이블</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static bool Delete(string tableName, string condition)
    {
        return m_OnChange($"DELETE FROM {tableName} WHERE {condition}");
    }
}

