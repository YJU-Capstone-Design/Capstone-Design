using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Web : MonoBehaviour
{
    public string url = "http://34.64.217.22/gameInfo.jsp";  // 이동할 웹사이트 URL
    [SerializeField] TextMeshProUGUI erro_Msg;
 

    public void OpenWebsite()
    {
        // 인터넷 연결이 되어 있는지 체크
        if (IsInternetAvailable())
        {
            Application.OpenURL(url); // 웹사이트로 이동
        }
        else
        {
            Debug.Log("인터넷에 연결되지 않았습니다.");
            // 연결 안 됐을 때 표시할 메시지나 UI 구현
            //ShowNoConnectionMessage();
        }
    }

    // 인터넷 연결 확인 함수 
    bool IsInternetAvailable()
    {
        try
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            using (client.OpenRead("http://www.google.com"))
            {
                return true; // 연결 가능
            }
        }
        catch
        {
            return false; // 연결 불가
        }
    }

    void ShowNoConnectionMessage()
    {
        // 연결이 안 되었을 때 사용자에게 보여줄 메시지
        // 예를 들어, 화면에 텍스트로 알려주기
        erro_Msg.text = "인터넷 연결이 되지 않았습니다.";
        
    }
}
