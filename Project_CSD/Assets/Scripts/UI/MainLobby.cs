using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


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





    private void Start()
    {


        toggle_anim = toggleBtn.GetComponent<Animator>();
        toggle_BG_anim = panel_Bg.GetComponent<Animator>();
        Clear();
    }


    public void OpenScene(string type)
    {
        int openScene = 0;
        if (type == "Gacha")
        {
            openScene = 1;
        }
        else if (type == "TraningBtn")
        {
            openScene = 2;
        }
        else if (type == "Formation")
        {
            openScene = 3;
        }
        else if (type == "KitchenRoom")
        {
            openScene = 4;
        }
        else if (type == "Collection")
        {
            openScene = 5;
        }
        else if (type == "Chm&Mission")
        {
            openScene = 6;
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


    public void ToggleOnOff()
    {
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
        
        toogleState = 0;
        mainLobby.transform.localScale = Vector3.one;
        menu[0].gameObject.SetActive(true);
        menu_Obj_Setting[0].gameObject.SetActive(true);
        setMenu.SetActive(false);

        for (int i = 1; i < menu.Count; i++)
        {
            menu[i].gameObject.SetActive(false);
            menu_Obj_Setting[i].gameObject.SetActive(false);
        }

    }
    public void SettingClose()
    {
        setMenu.SetActive(false);
    }
    public void GameSetting()
    {
        if (!setMenu.activeSelf)
        {
            setMenu.SetActive(true);
        }
        else
        {

        }
    }


    public void GameEscape()
    {
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