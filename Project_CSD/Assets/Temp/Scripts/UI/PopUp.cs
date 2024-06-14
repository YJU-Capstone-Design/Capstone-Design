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
        if (AudioManager.instance != null)
        {
            // AudioManager 인스턴스가 존재하면 ButtonSound 메서드 호출
            AudioManager.instance.ButtonSound();
        }
        stop.SetActive(true);
        menu.SetActive(true);
        Time.timeScale = 0;
    }
    public void MenuBtn(string type)
    {
        if (AudioManager.instance != null)
        {
            // AudioManager 인스턴스가 존재하면 ButtonSound 메서드 호출
            AudioManager.instance.ButtonSound();
        }

        if (type.Equals("Setting"))
        {

        }
        else if (type.Equals("Title"))
        {
            popUp_text.text = "로비로 이동  하시 겠습니까?";
            goLobby = true;
            pop.SetActive(true);
        }
        else if (type.Equals("Restart"))
        {
            popUp_text.text = "게임을 재시작   하시겠습니까?";
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
        if (AudioManager.instance != null)
        {
            // AudioManager 인스턴스가 존재하면 ButtonSound 메서드 호출
            AudioManager.instance.ButtonSound();
        }
        other.SetActive(false);
        stop.SetActive(false);
        
       
    }
    public void ScenceEscape()
    {
        if (AudioManager.instance != null)
        {
            // AudioManager 인스턴스가 존재하면 ButtonSound 메서드 호출
            AudioManager.instance.ButtonSound();
        }
        Time.timeScale = 1;
    }
    public void Continue()
    {
        if (AudioManager.instance != null)
        {
            // AudioManager 인스턴스가 존재하면 ButtonSound 메서드 호출
            AudioManager.instance.ButtonSound();
        }
        UiManager.Instance.time = 3;
        UiManager.Instance.SpeedUp();
    }

    public void MoveScene()
    {

        if (AudioManager.instance != null)
        {
            // AudioManager 인스턴스가 존재하면 ButtonSound 메서드 호출
            AudioManager.instance.ButtonSound();
        }
        if (goLobby)
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
        if (AudioManager.instance != null)
        {
            // AudioManager 인스턴스가 존재하면 ButtonSound 메서드 호출
            AudioManager.instance.ButtonSound();
        }

        LodingSceneMgr.LoadScene("MainLobby");
            Time.timeScale = 1;
        if (AudioManager.instance != null)
        {
            // AudioManager 인스턴스가 존재하면 ButtonSound 메서드 호출
            AudioManager.instance.MainSound();
        }
    }
}
