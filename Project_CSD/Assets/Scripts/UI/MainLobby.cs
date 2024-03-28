using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class MainLobby : MonoBehaviour
{
    [SerializeField] private List<GameObject> menu = new List<GameObject>();
    [SerializeField] private List<GameObject> menu_Obj_Setting = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI toggle;
    [SerializeField] private GameObject toggleMenu;

    [Header("toggle Transform")]
    [SerializeField] private GameObject toggleBtn;
    [SerializeField] private GameObject closeToggleBtn;
    [SerializeField] private GameObject panel_Bg;
    private Vector3 openToggleTr;//토클의 처음 위치
    private Vector3 closeToggleTr;//토클이 닫혔을 때의 위치

    [Header("SettingMenu")]
    [SerializeField] private GameObject setMenu;
    private void Awake()
    {
        Clear();
        openToggleTr = toggleBtn.transform.localPosition;
        closeToggleTr = closeToggleBtn.transform.localPosition;
        
    }

    public void OpenScene(string type)
    {
        int openScene = 0;
        if(type == "Gacha")
        {
            openScene = 1;
        }else if(type == "TraningBtn")
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
        for(int i=0; i<menu.Count; i++)
        {
            menu[i].gameObject.SetActive(false);
            menu_Obj_Setting[i].gameObject.SetActive(false);
        }
        menu[openScene].SetActive(true);
        menu_Obj_Setting[openScene].SetActive(true);
    }


    public void ToggleOnOff()
    {
        if(toggle.text == "<")
        {
            toggleMenu.SetActive(false);
            panel_Bg.SetActive(false);
            toggle.text = ">";
            toggleBtn.transform.localPosition = closeToggleTr;
        }
        else
        {
            toggleMenu.SetActive(true);
            panel_Bg.SetActive(true);
            toggle.text = "<";
            toggleBtn.transform.localPosition = openToggleTr;
        }
    }


    public void Clear()
    {
        menu[0].gameObject.SetActive(true);
        menu_Obj_Setting[0].gameObject.SetActive(true);
        setMenu.SetActive(false);
        for (int i=1; i<menu.Count; i++)
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
        Application.Quit();
    }
    
}
