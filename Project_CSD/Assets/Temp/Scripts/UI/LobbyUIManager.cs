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
    [SerializeField] GameObject loginUI; //로그인 창
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

    public void LoginButton()//로그인 창 id글자는 5개 제한 번호는 숫자만으로 제한됨
    {
        if (!string.IsNullOrEmpty(playerNameInput.text) && !string.IsNullOrEmpty(playerPwdInput.text))
        {
            string playerName = playerNameInput.text;
            int playerPwd = int.Parse(playerPwdInput.text);

            // 비속어 필터링
            if (NewBehaviourScript.Instance.CheckWord(playerName))
            {
                OpenUI("alert");
                alertText.text = "사용할 수 없는 이름입니다.";
            }
            else
            {
                // 입력한 이름이 DB 에 있는 지 확인
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
                        alertText.text = "비밀번호가 잘못되었습니다.";
                    }
                }
                else
                {
                    GameStart();

                    // 새로운 User 데이터 DB에 입력
                    DBConnect.Insert("account", $"'{playerName}', {playerPwd}");
                }
            }
        }
        else if (string.IsNullOrEmpty(playerNameInput.text) || (!string.IsNullOrEmpty(playerPwdInput.text) && string.IsNullOrEmpty(playerNameInput.text)))
        {
            OpenUI("alert");
            alertText.text = "이름을 입력해주세요.";
        }
        else if (string.IsNullOrEmpty(playerPwdInput.text))
        {
            OpenUI("alert");
            alertText.text = "비밀번호를 입력해주세요.";
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

        // 사운드
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

        // 사운드
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
    }

    public void AddUsingCard(int cardID)
    {

        //액셀에 카드 아이디랑 값은 컬럼에 count += 1;
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

    // 유닛 도감 버튼(유닛 카드) 함수
    public void GetUnitInfo(UnitData unitData)
    {
        unitNameText.text = unitData.UnitName;
        unitHPText.text = unitData.Health.ToString();
        unitPowerText.text = unitData.Power.ToString();
        unitCostText.text = unitData.Cost.ToString();
        unitSpeedText.text = unitData.MoveSpeed.ToString();
        unitAtkSpeedText.text = unitData.AttackTime.ToString();

        // 유닛 Spine UI
        unitGraphic.SetActive(false);
        SkeletonGraphic unitSkeletonGraphic = unitGraphic.GetComponent<SkeletonGraphic>();
        unitSkeletonGraphic.skeletonDataAsset = unitData.Unit_skeletonData;
        unitSkeletonGraphic.Initialize(true); // skeletonDataAsset 를 ReLoad
        unitSkeletonGraphic.startingLoop = true;
        unitGraphic.SetActive(true);
    }
}
