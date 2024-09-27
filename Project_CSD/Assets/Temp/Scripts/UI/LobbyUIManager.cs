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

    [Header("# Collection")]
    [SerializeField] GameObject unitCollectionUI;
    [SerializeField] GameObject speelCollectionUI;
    [SerializeField] TextMeshProUGUI unitNameText;
    [SerializeField] TextMeshProUGUI unitHPText;
    [SerializeField] TextMeshProUGUI unitPowerText;
    [SerializeField] TextMeshProUGUI unitCostText;
    [SerializeField] TextMeshProUGUI unitSpeedText;
    [SerializeField] TextMeshProUGUI unitAtkSpeedText;
    [SerializeField] GameObject unitGraphic;

    private void Awake()
    {
        loginUI.SetActive(false);
        alertUI.SetActive(false);
        helpUI.SetActive(false);
        //CollectionClear();
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

        /*SceneMgr.Instance.GoSceneSelect("NomalMode");*/
        SceneManager.LoadScene("NormalBattle");
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
            case "unitCollection":
                unitCollectionUI.SetActive(true);
                speelCollectionUI.SetActive(false);
                break;
            case "spellCollection":
                unitCollectionUI.SetActive(false);
                speelCollectionUI.SetActive(true);
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
            case "collection":
                CollectionClear();
                break;
        }

        // ����
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
    }

    public void AddUsingCard(int cardID)
    {

        //�׼��� ī�� ���̵�� ���� �÷��� count += 1;
    }


    void CollectionClear()
    {
        unitCollectionUI.SetActive(false);
        speelCollectionUI.SetActive(false);
        unitNameText.text = "";
        unitHPText.text = "";
        unitPowerText.text = "";
        unitCostText.text = "";
        unitSpeedText.text = "";
        unitAtkSpeedText.text = "";
        unitGraphic.SetActive(false);

        if (unitGraphic.GetComponent<SkeletonGraphic>().SkeletonDataAsset != null)
        {
            unitGraphic.GetComponent<SkeletonGraphic>().SkeletonDataAsset.Clear();
        }
    }

    // ���� ���� ��ư(���� ī��) �Լ�
    public void GetUnitInfo(UnitData unitData)
    {
        unitNameText.text = unitData.UnitName;
        unitHPText.text = unitData.Health.ToString();
        unitPowerText.text = unitData.Power.ToString();
        unitCostText.text = unitData.Cost.ToString();
        unitSpeedText.text = unitData.MoveSpeed.ToString();
        unitAtkSpeedText.text = unitData.AttackTime.ToString();

        // ���� Spine UI
        unitGraphic.SetActive(false);
        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();
        unitSkeletonGraphic.skeletonDataAsset = unitData.Unit_skeletonData;
        unitSkeletonGraphic.Initialize(true); // skeletonDataAsset �� ReLoad
        unitSkeletonGraphic.startingLoop = true;
        unitGraphic.SetActive(true);
    }
}
