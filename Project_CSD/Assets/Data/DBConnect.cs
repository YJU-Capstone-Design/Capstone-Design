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

    // MySql 연결 함수
    public bool Connection()
    {
        string conStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode=none;",
         "34.64.201.214", "cst", "root", "yju123", "3306");

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

    public static XmlNodeList SelectOriginal(string tableName, string query)
    {
        DataSet dataSet = m_OnLoad(tableName, query);

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
    public static bool Insert(string tableName, string value)
    {
        return m_OnChange($"INSERT INTO {tableName} VALUES ({value})");
    }
    public static bool Insert(string tableName, string fieldName, string value)
    {
        return m_OnChange($"INSERT INTO {tableName} ({fieldName}) VALUES ({value})");
    }

    // UserData 테이블 Insert
    public static bool UserDataInsert(string userName, int wave)
    {
        string strValues = string.Empty;

        strValues = $"'{userName}',";

        // 도달 라운드 Value 값
        for (int i = 0; i < wave; i++)
        {
            if(i == wave - 1 && wave == 11)
            {
                strValues += "1";
            }
            else
            {
                strValues += "1, ";
            }
        }

        // 도달 실패 라운드 Value 값
        for(int i = wave; i < 11; i++)
        {
            if (i == 10)
            {
                strValues += "0";
            }
            else
            {
                strValues += "0, ";
            }
        }

        Debug.Log(strValues);
        Debug.Log("Insert userData");
        return m_OnChange($"INSERT INTO userData VALUES ({strValues})");
    }

    /// <summary>
    /// 레코드 갱신
    /// </summary>
    /// <param name="tableName">입력할 테이블</param>
    /// <param name="fieldName">입력할 필드 이름</param>
    /// <param name="value">입력할 값</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static bool UpdateOriginal(string query)
    {
        Debug.Log("Update Data");
        return m_OnChange(query);
    }
    public static bool UpdateRanking(string tableName, string fieldName, int score, string condition)
    {
        Debug.Log("Update Ranking Data");
        return m_OnChange($"UPDATE {tableName} SET {fieldName}={score} WHERE {condition}");
    }

    // UserData 테이블 Update
    public static bool UserDataUpdate(string userName, int wave)
    {
        string strValues = string.Empty;

        // 도달 라운드 Value 값
        for (int i = 1; i < wave; i++)
        {
            if (i == wave - 1 && wave == 11)
            {
                strValues += $"stage_clear = 1";
            }
            else
            {
                strValues += $"stage_{i + 1} = 1, ";
            }
        }

        // 도달 실패 라운드 Value 값
        for (int i = wave; i < 11; i++)
        {
            if (i == 10)
            {
                strValues += $"stage_clear = 0";
            }
            else
            {
                strValues += $"stage_{i + 1} = 0, ";
            }
        }

        Debug.Log(strValues);
        Debug.Log("Update userData");
        return m_OnChange($"UPDATE userData SET {strValues} WHERE userName = '{userName}'");
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

