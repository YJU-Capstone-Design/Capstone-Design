using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    [Header("# Login UI")]
    public TMP_InputField playerNameInput;
    [SerializeField] TMP_InputField playerPwdInput;
    [SerializeField] GameObject loginUI; //�α��� â
    [SerializeField] GameObject alertUI;
    [SerializeField] TextMeshProUGUI alertText;

    [Header("# etc UI")]
    [SerializeField] GameObject helpUI;

    private void Awake()
    {
        loginUI.SetActive(false);
        alertUI.SetActive(false);
        helpUI.SetActive(false);
    }

    public void LoginButton()//�α��� â id���ڴ� 5�� ���� ��ȣ�� ���ڸ����� ���ѵ�
    {
        if (!string.IsNullOrEmpty(playerNameInput.text) && !string.IsNullOrEmpty(playerPwdInput.text))
        {
            string playerName = playerNameInput.text;
            int playerPwd = int.Parse(playerPwdInput.text);

            // ��Ӿ� ���͸�
            if (NewBehaviourScript.Instance.CheckWord(playerName))
            {
                OpenUI("alert");
                alertText.text = "����� �� ���� �̸��Դϴ�.";
            }
            else
            {
                // �Է��� �̸��� DB �� �ִ� �� Ȯ��
                XmlNodeList user = DBConnect.Select("account", $"WHERE userName = '{playerName}'");

                if (user != null)
                {
                    if (playerPwd == int.Parse(user[0]["password"].InnerText))
                    {
                        GameStart();
                    }
                    else
                    {
                        OpenUI("alert");
                        alertText.text = "��й�ȣ�� �߸��Ǿ����ϴ�.";
                    }
                }
                else
                {
                    GameStart();

                    // ���ο� User ������ DB�� �Է�
                    DBConnect.Insert("account", $"'{playerName}', {playerPwd}");
                }
            }
        }
        else if (string.IsNullOrEmpty(playerNameInput.text) || (!string.IsNullOrEmpty(playerPwdInput.text) && string.IsNullOrEmpty(playerNameInput.text)))
        {
            OpenUI("alert");
            alertText.text = "�̸��� �Է����ּ���.";
        }
        else if (string.IsNullOrEmpty(playerPwdInput.text))
        {
            OpenUI("alert");
            alertText.text = "��й�ȣ�� �Է����ּ���.";
        }
    }

    void GameStart()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        UserRankingData.instance.playerName = playerNameInput.text;

        SceneMgr.Instance.GoSceneSelect("NomalMode");
       // SceneManager.LoadScene("NormalBattle");
    }

    public void OpenUI(string UIName)
    {
        switch (UIName)
        {
            case "help":
                helpUI.SetActive(true);
                break;
            case "login":
                loginUI.SetActive(true);
                break;
            case "alert":
                alertUI.SetActive(true);
                break;
        }

        // ����
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
    }

    public void CloseUI(string UIName)
    {
        switch (UIName)
        {
            case "help":
                helpUI.SetActive(false);
                break;
            case "login":
                loginUI.SetActive(false);
                break;
            case "alert":
                alertUI.SetActive(false);
                break;
        }

        // ����
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
    }

    public void AddUsingCard(int cardID)
    {

        //�׼��� ī�� ���̵�� ���� �÷��� count += 1;
    }
}
