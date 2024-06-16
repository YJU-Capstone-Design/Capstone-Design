using JetBrains.Annotations;
using Spine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

using UnityEngine.UI;


public class MainLobby : MonoBehaviour
{
    [SerializeField] private List<GameObject> menu = new List<GameObject>();
    [SerializeField] private List<GameObject> menu_Obj_Setting = new List<GameObject>();
    [SerializeField] private GameObject toggleMenu;
    private int toogleState = 0;
    private int roomNum = 0;//룸번호

    [Header("toggle Transform")]
    [SerializeField] private GameObject toggleBtn;
    [SerializeField] private GameObject closeToggleBtn;
    [SerializeField] private GameObject toggleBG;
    [SerializeField] private GameObject panel_Bg;
    private int toggleCheck = 0;

    Animator toggle_anim;
    Animator toggle_BG_anim;
    [Header("SettingMenu")]
    [SerializeField] private GameObject setMenu;


    [Header("BattleModeSelect")]
    [SerializeField] private GameObject battleMode;


    [Header("Lobby")]
    [SerializeField] private GameObject mainLobby;//메인로비 UI(캔버스)
    [SerializeField] private GameObject mainLobbyObj;//메인로비 필드 오브젝트

    [Header("PlayerInfo")]//플레이어 정보
    public GameObject playerCard;
    public TextMeshProUGUI nick;
    public TextMeshProUGUI lv;
    [SerializeField]  Image player_icon;

    [Header("PlayerCard")]//플레이어 카드 정보
    public TextMeshProUGUI card_nick;
    public TextMeshProUGUI card_lv;
    public Image card_icon;
    public TextMeshProUGUI Char_cnt;
    public TextMeshProUGUI Spell_cnt;
    public TextMeshProUGUI cash;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI uid;
    public HoldingList holdingList;

    [Header("MainBg")]//진입금지룸
    [SerializeField] GameObject mainBG;


    [Header("Prohibited Room")]//진입금지룸
    [SerializeField] GameObject pop;

    private void Start()
    {


        toggle_anim = toggleBtn.GetComponent<Animator>();
        toggle_BG_anim = panel_Bg.GetComponent<Animator>();
        Clear();
        PlayerInfo();

    }
    public void PlayerInfo()
    {
        if (PlayerData.instance != null) {
            
            nick.text = PlayerData.instance.NAME;
            lv.text = PlayerData.instance.Lv.ToString();
            player_icon.sprite = PlayerData.instance.icon;
        }
    }
    public void OpenPlayerCard()
    {
        playerCard.SetActive(true);
        if (PlayerData.instance != null)
        {

            card_nick.text = PlayerData.instance.NAME;
            card_lv.text = "Lv "+PlayerData.instance.Lv.ToString();
            card_icon.sprite = PlayerData.instance.icon;
            Char_cnt.text = "캐릭터 "+ PlayerData.instance.Char.ToString()+" / "+GachaManager.single.listGachaTemplete.Count;
            Spell_cnt.text = "스펠 "+PlayerData.instance.Spell.ToString() + " / " + GachaManager.single.listSpellItem.Count;
            cash.text = "캐쉬 "+CashManager.instance.player_Cash.ToString();
            gold.text = "골드 "+CashManager.instance.player_Gold.ToString();
            uid.text ="uid "+ PlayerData.instance.Player_No.ToString();
        }
    }
    public void ClosePlayerCard() { playerCard.SetActive(false); }

    public void OpenScene(string type)
    {

        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }


        int openScene = 0;
        if (type.Equals("Gacha"))
        {
            mainBG.SetActive(false);
            openScene = 1;
            if (AudioManager.instance != null) { AudioManager.instance.GachaSound(); }
        }
        else if (type.Equals("TraningBtn"))
        {
            pop.SetActive(true);
            //openScene = 2;
        }
        else if (type.Equals("Formation"))
        {
            pop.SetActive(true);
            //openScene = 3;
        }
        else if (type.Equals("KitchenRoom"))
        {
            pop.SetActive(true);
            // openScene = 4;
        }
        else if (type.Equals("Collection"))
        {
            openScene = 5;
        }
        else if (type.Equals("Chm&Mission"))
        {
            pop.SetActive(true);
            //openScene = 6;
        }
        for (int i = 1; i < menu.Count; i++)
        {
            
                menu[i].gameObject.SetActive(false);
                menu_Obj_Setting[i].gameObject.SetActive(false);
                mainLobby.transform.localScale = Vector3.zero;
                mainLobbyObj.transform.localScale = Vector3.zero;
            
          
        }
       menu[openScene].SetActive(true);
       menu_Obj_Setting[openScene].SetActive(true);
       
      
        roomNum = 1;
        if (openScene == 0)
        {
            
            mainLobby.transform.localScale = Vector3.one;
            mainLobbyObj.transform.localScale = Vector3.one;
            roomNum = 0;
        }

       
       
        
    }

    public void ClosePoP()
    {

        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }

        pop.SetActive(false);
    }
    public void ToggleOnOff()
    {

        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }

        if (toogleState == 0)
        {

            toggleMenu.SetActive(false);
            //panel_Bg.SetActive(false);

            toogleState = 1;
            //toggleBtn.transform.localPosition = closeToggleTr;
            toggle_BG_anim.SetInteger("ToggleState", 0);
            toggle_BG_anim.SetBool("ToggleSet", false);
            toggle_anim.SetInteger("ToggleState", 0);
            toggle_anim.SetBool("ToggleSet", false);

           
        }
        else if (toogleState == 1)
        {
            toggle_BG_anim.SetInteger("ToggleState", 1);
            toggle_BG_anim.SetBool("ToggleSet", true);
            toggle_anim.SetInteger("ToggleState", 1);
            toggle_anim.SetBool("ToggleSet", true);
            Invoke("OnToggle", 0.5f);
            toogleState = 0;

            //toggleBtn.transform.localPosition = openToggleTr;

        }
    }

    public void BtlModeSelect()
    {

        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }

        if (!battleMode.activeSelf)
        {
            battleMode.SetActive(true);
        }
        else if (battleMode.activeSelf)
        {
            battleMode.SetActive(false);
        }
    }
    public void OnToggle()
    {
       
        toggleMenu.SetActive(true);


    }

    public void Clear()
    {
        mainBG.SetActive(true);
        toogleState = 0;
        mainLobby.transform.localScale = Vector3.one;
        menu[0].gameObject.SetActive(true);
        menu_Obj_Setting[0].gameObject.SetActive(true);
        
        roomNum = 0;
        for (int i = 1; i < menu.Count; i++)
        {
            
                menu[i].gameObject.SetActive(false);
                menu_Obj_Setting[i].gameObject.SetActive(false);

           
        }

    }
 
    public void GameSetting()
    {
        if (AudioManager.instance != null) { AudioManager.instance.OpenAudioBox(); }

       
    }


    public void GameEscape()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }

        if (roomNum == 0)
        {

            Application.Quit();
            Debug.Log("Game End");
        }
        else
        {
            Clear();
        }
      
    }

}