using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{

    
    public void GoSceneSelect(string type)
    {
        if (type == "BattleModeSelect")
        {
            SceneManager.LoadScene("MainLobby");
        }
        else if (type == "MainLobbySelect")
        {
            SceneManager.LoadScene("MainLobby");
        }else if(type == "NomalMode")
        {
            SceneManager.LoadScene("NormalBattle");
        }

    }
}
