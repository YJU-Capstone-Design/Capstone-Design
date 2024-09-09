using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserRankingData : MonoBehaviour
{
    public static UserRankingData instance;

    [Header("���� ������ ��ŷ ����")]
    public int stage = 1; //���� ��������
    public int clear_Stage = 1; // Ŭ������ ��������
    public int score;
    
    [Header("�÷��̾� ����")]
    [SerializeField] TMP_InputField player_Name;
    [SerializeField] TMP_InputField player_No;
    [SerializeField] GameObject loginBox;//�α��� â
    public string playerName;//�̸�
    public int playerNo;//�й�

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
        loginBox.SetActive(false); 
    }




    public void Login()//�α��� â id���ڴ� 5�� ���� ��ȣ�� ���ڸ����� ���ѵ�
    {
        if (player_Name != null && player_No !=null)
        {
            if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
            playerName = player_Name.text;
            playerNo = int.Parse(player_No.text);
            
            SceneMgr.Instance.GoSceneSelect("NomalMode");
        }
    }
    public void AddUsingCard(int cardID)
    {

        //�׼��� ī�� ���̵�� ���� �÷��� count += 1;
    }
    public void Close()//�α��� â ������
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        loginBox.SetActive(false);
    }
    public void OpenLogin()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        loginBox.SetActive(true);
    }

}
