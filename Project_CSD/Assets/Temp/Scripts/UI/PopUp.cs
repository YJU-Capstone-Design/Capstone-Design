using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject stop;

    [Header("PoPMenu")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject pop;
    [SerializeField] private GameObject result_Lobby;
    [SerializeField] private TextMeshProUGUI popUp_text;

    private bool goLobby;

    private void Awake()
    {
        Clear();
    }
    public void OpenMenu()
    {
        stop.SetActive(true);
        menu.SetActive(true);
        Time.timeScale = 0;
    }
    public void MenuBtn(string type)
    {
        
        if (type.Equals("Setting"))
        {

        }
        else if (type.Equals("Title"))
        {
            popUp_text.text = "�κ�� �̵�  �Ͻ� �ڽ��ϱ�?";
            goLobby = true;
            pop.SetActive(true);
        }
        else if (type.Equals("Restart"))
        {
            popUp_text.text = "������ �����   �Ͻðڽ��ϱ�?";
            goLobby = false;
            pop.SetActive(true);
        }
    }
    public void Clear()
    {
        stop.SetActive(false);
        menu.SetActive(false);
        pop.SetActive(false);
        Time.timeScale = 1;
    }

    public void Close(GameObject other)
    {
        other.SetActive(false);
        stop.SetActive(false);
        
       
    }
    public void ScenceEscape()
    {
        Time.timeScale = 1;
    }
    public void Continue()
    {
        UiManager.Instance.time = 3;
        UiManager.Instance.SpeedUp();
    }

    public void MoveScene()
    {
        
       
        if(goLobby)
        {
            LodingSceneMgr.LoadScene("MainLobby");
            Time.timeScale = 1;
        }
        else if (!goLobby)
        {
            SceneMgr.Instance.GoSceneSelect("NomalMode");
        }
    }
    public void MoveScene2()
    {
        
            LodingSceneMgr.LoadScene("MainLobby");
            Time.timeScale = 1;
        
    }
}