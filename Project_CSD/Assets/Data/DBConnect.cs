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
    //    //"127.0.0.1", "testDB", "root", "1478", "3306");    // -> 내 로컬 db

    //    Debug.Log("Connection String: " + conStr);  // 로그에 연결 문자열 출력

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


    // GCP 서버의 API 주소 (예시)
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
    //    string serverPath = "http://34.64.201.214.com/LoadMySQL.php"; //PHP 파일의 위치를 저장

    //    WWWForm form = new WWWForm(); //Post 방식으로 넘겨줄 데이터(AddField로 넘겨줄 수 있음)

    //    using (UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form)) //웹 서버에 요청
    //    {
    //        yield return webRequest.SendWebRequest(); //요청이 끝날 때까지 대기

    //        Debug.Log(webRequest.downloadHandler.text); //서버로부터 받은 데이터를 string 형태로 출력
    //    }
    //}
}
