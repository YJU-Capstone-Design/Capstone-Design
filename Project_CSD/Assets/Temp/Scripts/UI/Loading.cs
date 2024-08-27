using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{
    public static Loading instance;
    string type = "";
    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    public void Loding(string type)
    {
        this.type = type;
        Invoke("MoveScene", 0.5f);


    }
    void MoveScene()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        switch (this.type)
        {
            case "Lobby":
                LodingSceneMgr.LoadScene("MainLobby");
                
                Debug.Log("Loding");
                break;
            case "BattleMode":
                LodingSceneMgr.LoadScene("BattleMode");
                
                Debug.Log("BattleMode");
                break;
            case "NomalMode":
                LodingSceneMgr.LoadScene("NormalBattle");
               
                BreakRackMgr.Instance.transform.localScale = Vector3.zero;
                Debug.Log("BattleMode");
                break;
        }
    }
}

