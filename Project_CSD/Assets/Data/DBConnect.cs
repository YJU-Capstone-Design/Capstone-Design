using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using UnityEngine.Networking;

public class DBConnect : MonoBehaviour
{
    //void Start()
    //{
    //    Debug.Log("Connection Test :" + Connection());
    //}


    //public bool Connection()
    //{
    //    string conStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode=none;",
    //     "34.64.201.214", "cst", "root", "yju123", "3306");
    //    //"34.64.201.214", "cst", "ori", "Asdf1478!", "3306");
    //    //"127.0.0.1", "testDB", "root", "1478", "3306");    // -> �� ���� db

    //    Debug.Log("Connection String: " + conStr);  // �α׿� ���� ���ڿ� ���

    //    try
    //    {
    //        Debug.Log("Attempting to open connection...");
    //        using (MySqlConnection con = new MySqlConnection(conStr))
    //        {
    //            con.Open();
    //        }
    //        Debug.Log("Connection successful.");
    //        return true;
    //    }
    //    catch (MySqlException ex)
    //    {
    //        Debug.LogError("MySQL Error: " + ex.Message);
    //        if (ex.InnerException != null)
    //        {
    //            Debug.LogError("Inner Exception: " + ex.InnerException.Message);
    //        }
    //        return false;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("General Error: " + ex.Message);
    //        if (ex.InnerException != null)
    //        {
    //            Debug.LogError("Inner Exception: " + ex.InnerException.Message);
    //        }
    //        return false;
    //    }
    //}


    // GCP ������ API �ּ� (����)
    //    private string apiUrl = "https://34.64.201.214/api/cst";


    //    private void Start()
    //    {
    //        Debug.Log("Starting SendGameResult coroutine.");
    //        StartCoroutine(SendGameResult("2001565", 10));
    //    }

    //    public IEnumerator SendGameResult(string playerId, int score)
    //    {
    //        var jsonData = JsonUtility.ToJson(new { player_id = playerId, score = score, game_time = System.DateTime.Now });

    //        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
    //        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
    //        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //        request.downloadHandler = new DownloadHandlerBuffer();
    //        request.SetRequestHeader("Content-Type", "application/json");

    //        yield return request.SendWebRequest();

    //        if (request.result == UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log("Request succeeded. Response: " + request.downloadHandler.text);
    //        }
    //        else
    //        {
    //            Debug.LogError("Request failed. Error: " + request.error);
    //        }
    //    }

    //private void Start()
    //{
    //    StartCoroutine(GetMySQLData());
    //}

    //private IEnumerator GetMySQLData()
    //{
    //    string serverPath = "http://34.64.201.214.com/LoadMySQL.php"; //PHP ������ ��ġ�� ����

    //    WWWForm form = new WWWForm(); //Post ������� �Ѱ��� ������(AddField�� �Ѱ��� �� ����)

    //    using (UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form)) //�� ������ ��û
    //    {
    //        yield return webRequest.SendWebRequest(); //��û�� ���� ������ ���

    //        Debug.Log(webRequest.downloadHandler.text); //�����κ��� ���� �����͸� string ���·� ���
    //    }
    //}
}
