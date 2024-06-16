using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberAdd : MonoBehaviour
{
    [Header("회원 가입")]
    public TMP_InputField id;
    public TMP_InputField pw;
    public TMP_InputField nick;
    [Header("로그인")]
    public TMP_InputField login_Id;
    public TMP_InputField login_Pw;
    [Header("로그인 팝업")]
    public TextMeshProUGUI loginPop;
    [Header("아이콘 선택")]
    public Sprite[] icon;
    private int iconValue;
    public Image selectIcon;

    [Header("회원 오브젝트")]
    public GameObject memberAdd;
    public GameObject login;

    public void OpenMemeberAdd()
    {
        AudioManager.instance.ButtonSound();
        memberAdd.SetActive(true);
    }
    public void CloseMemberAdd() {
        AudioManager.instance.ButtonSound();
        PlayerData.instance.AddMember(nick.text, id.text, pw.text, selectIcon.sprite);
        id.text = "";
        pw.text = "";
        nick.text = "";
        selectIcon.sprite = icon[0];
        memberAdd.SetActive(false);
    }
    public void CancelMemberAdd() {
        id.text = "";
        pw.text = "";
        nick.text = "";
        selectIcon.sprite = icon[0];
        AudioManager.instance.ButtonSound(); memberAdd.SetActive(false); 
    }
    public void Login()
    {
        string memberId = PlayerData.instance.ID;
        string memberPw = PlayerData.instance.PASS;
        if (login_Id.text.Equals(memberId)&& login_Pw.text.Equals(memberPw))
        {
            loginPop.text = "";
            Loading.instance.Loding("Lobby");
        }
        else
        {
             AudioManager.instance.ButtonSound(); 
            loginPop.text = "아이디 또는 비밀번호가 틀렸습니다.";
        }
    }
    public void OpenLogin()
    {
        AudioManager.instance.ButtonSound();
        login.SetActive(true);
    }
    public void CloseLogin()
    {
        AudioManager.instance.ButtonSound();
        loginPop.text = "";
        login_Id.text = "";
        login_Pw.text = "";
        login.SetActive(false);
    }
    public void IconChangeRight()
    {
        AudioManager.instance.ButtonSound();
        iconValue++;
        if (icon.Length <= iconValue) { iconValue = 0; }
        

        selectIcon.sprite = icon[iconValue];
    }
    public void IconChangeLeft()
    {
        AudioManager.instance.ButtonSound();
        iconValue--;
        if (iconValue< 0) { iconValue = 0; }
        

        selectIcon.sprite = icon[iconValue];
    }

}
