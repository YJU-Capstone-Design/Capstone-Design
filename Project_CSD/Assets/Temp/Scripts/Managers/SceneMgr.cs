using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{

    
    public void GoSceneSelect(string type)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
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
            AudioManager.instance.lobbyscene = false;
            LodingSceneMgr.LoadScene("NormalBattle");
            //SceneManager.LoadScene("NormalBattle");
            
        }

    }


    public void SoundControl()
    {

    }
}
