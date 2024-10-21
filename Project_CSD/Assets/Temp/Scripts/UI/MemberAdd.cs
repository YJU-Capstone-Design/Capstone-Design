using Spine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MemberAdd : MonoBehaviour
{
    [Header("ȸ�� ����")]
    public TMP_InputField id;
    public TMP_InputField pw;
    public TMP_InputField nick;
    [Header("�α���")]
    public TMP_InputField login_Id;
    public TMP_InputField login_Pw;
    [Header("�α��� �˾�")]
    public TextMeshProUGUI loginPop;
    [Header("������ ����")]
    public Sprite[] icon;
    private int iconValue;
    public Image selectIcon;

    [Header("ȸ�� ������Ʈ")]
    public GameObject memberAdd;
    public GameObject login;

    [Header("Mouse Cursor")]
    public Texture2D normalCursor; // �⺻ Ŀ�� �̹���
    public Texture2D clickCursor; // Ŭ�� �� �ٲ� Ŀ�� �̹���
    private void Awake()
    {

        // �⺻ Ŀ�� ����
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);

    }
    private void Update()
    {
        // ���콺 Ŭ�� ��
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
        }

        // ���콺 ��ư�� ����
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }

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
            loginPop.text = "���̵� �Ǵ� ��й�ȣ�� Ʋ�Ƚ��ϴ�.";
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
