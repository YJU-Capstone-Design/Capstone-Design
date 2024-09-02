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
    private static MySqlConnection connection
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
        DataSet ds = null; // DataSet : 여러 테이블을 메모리 상에 담을 수 있는 데이터 구조
        try
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = query;

            MySqlDataAdapter sd = new MySqlDataAdapter(cmd);  // MySqlDataAdapter : 데이터베이스로부터 데이터를 가져옴.
            ds = new DataSet();       // DataSet 객체를 새로 생성한 후, MySqlDataAdapter의 Fill 메서드를 통해 쿼리 결과를 DataSet에 넣
            sd.Fill(ds, tableName);   // tableName 매개변수를 사용해 DataSet 내의 특정 테이블에 데이터를 채움.
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
        return ds;
    }

    /// <summary>
    /// 데이터 검색
    /// </summary>
    /// <param name="tableName">검색할 테이블</param>
    /// <param name="field">검색할 필드 (입력하지 않을 경우 전체 로드)</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static XmlNodeList Select(string tableName, string field = "*", string condition = "")
    {
        DataSet dataSet = m_OnLoad(tableName, $"SELECT {field} FROM {tableName} {condition}");

        if (dataSet == null)
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
    public static bool Update(string tableName, string fieldName, string value, string condition)
    {
        Debug.Log("Update Data");
        return m_OnChange($"UPDATE {tableName} SET {fieldName}='{value}' WHERE {condition}");
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

