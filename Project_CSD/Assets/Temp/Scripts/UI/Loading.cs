using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Loading : Singleton<Loading>
{
    string type = "";
    // Use this for initialization
    public void Loding(string type)
    {
        this.type = type;
        Invoke("MoveScene", 0.5f);


    }
    void MoveScene()
    {
        AudioManager.instance.ButtonSound();
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
                Debug.Log("BattleMode");
                break;
        }
    }
}

