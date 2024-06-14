using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{

    
    public void GoSceneSelect(string type)
    {
        AudioManager.instance.BattleSound();
        if (type.Equals("BattleModeSelect"))
        {
            SceneManager.LoadScene("MainLobby");
        }
        else if (type.Equals("MainLobbySelect"))
        {
          
            SceneManager.LoadScene("MainLobby");
          
        }
        else if(type.Equals("NomalMode"))
        {
           
            SceneManager.LoadScene("NormalBattle");
            
        }

    }


    public void SoundControl()
    {

    }
}
