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

    [Header("���� ������ ��ŷ ����")]
    public int stage = 1; //���� ��������
    public int clear_Stage = 1; // Ŭ������ ��������
    public int score = 0;
    
    [Header("�÷��̾�/�α���")]
    public string playerName; //�̸�
    public int playerPwd; //��й�ȣ

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
