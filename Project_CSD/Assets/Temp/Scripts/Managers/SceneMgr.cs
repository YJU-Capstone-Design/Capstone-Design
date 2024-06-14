using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{

    
    public void GoSceneSelect(string type)
    {
        if (AudioManager.instance != null)
        {
            // AudioManager 인스턴스가 존재하면 BattleSound 메서드 호출
            AudioManager.instance.ButtonSound();
        }
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
