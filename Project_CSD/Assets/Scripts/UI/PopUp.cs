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
    [SerializeField] private TextMeshProUGUI popUp_text;

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
            popUp_text.text = "로비로 이동하시겠습니까?";
            pop.SetActive(true);
        }
        else if (type.Equals("Restart"))
        {
            popUp_text.text = "게임을 재 시작하시겠습니까?";
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
        Time.timeScale = 1;
    }

    public void MoveScene()
    {
        string type = popUp_text.text.ToString();
        Debug.Log(type);
        if(type == "로비로 이동하시겠습니까?")
        {
            SceneMgr.Instance.GoSceneSelect("MainLobbySelect");
        }
        else if (type == "게임을 재 시작하시겠습니까?")
        {
            SceneMgr.Instance.GoSceneSelect("NomalMode");
        }
    }
}
