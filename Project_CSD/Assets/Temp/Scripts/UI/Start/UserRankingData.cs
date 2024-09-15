using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UserRankingData : MonoBehaviour
{
    public static UserRankingData instance;

    [Header("유저 데이터 랭킹 변수")]
    public int stage = 1; //현재 스테이지
    public int clear_Stage = 1; // 클리어한 스테이지
    public int score = 0;
    
    [Header("플레이어/로그인")]
    public string playerName; //이름
    public int playerPwd; //비밀번호

    private void Awake()
    {
        instance = this;
        var obj = FindObjectsOfType<UserRankingData>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
