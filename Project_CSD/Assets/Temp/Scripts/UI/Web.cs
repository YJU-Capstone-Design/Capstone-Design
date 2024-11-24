using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Web : MonoBehaviour
{
    public string url = "http://34.64.217.22/gameInfo.jsp";  // �̵��� ������Ʈ URL
    [SerializeField] TextMeshProUGUI erro_Msg;
 

    public void OpenWebsite()
    {
        // ���ͳ� ������ �Ǿ� �ִ��� üũ
        if (IsInternetAvailable())
        {
            Application.OpenURL(url); // ������Ʈ�� �̵�
        }
        else
        {
            Debug.Log("���ͳݿ� ������� �ʾҽ��ϴ�.");
            // ���� �� ���� �� ǥ���� �޽����� UI ����
            //ShowNoConnectionMessage();
        }
    }

    // ���ͳ� ���� Ȯ�� �Լ� 
    bool IsInternetAvailable()
    {
        try
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            using (client.OpenRead("http://www.google.com"))
            {
                return true; // ���� ����
            }
        }
        catch
        {
            return false; // ���� �Ұ�
        }
    }

    void ShowNoConnectionMessage()
    {
        // ������ �� �Ǿ��� �� ����ڿ��� ������ �޽���
        // ���� ���, ȭ�鿡 �ؽ�Ʈ�� �˷��ֱ�
        erro_Msg.text = "���ͳ� ������ ���� �ʾҽ��ϴ�.";
        
    }
}
