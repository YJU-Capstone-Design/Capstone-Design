using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserRankingData : MonoBehaviour
{
    public static UserRankingData instance;

    [Header("유저 데이터 랭킹 변수")]
    public int stage = 1; //현재 스테이지
    public int clear_Stage = 1; // 클리어한 스테이지
    public int score;
    
    [Header("플레이어 정보")]
    [SerializeField] TMP_InputField player_Name;
    [SerializeField] TMP_InputField player_No;
    [SerializeField] GameObject loginBox;//로그인 창
    public string playerName;//이름
    public int playerNo;//학번

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




    public void Login()//로그인 창 id글자는 5개 제한 번호는 숫자만으로 제한됨
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

        //액셀에 카드 아이디랑 값은 컬럼에 count += 1;
    }
    public void Close()//로그인 창 나가기
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
