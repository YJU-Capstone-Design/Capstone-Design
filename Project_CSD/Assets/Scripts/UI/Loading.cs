using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{

    // Use this for initialization
    public void Loding(string type)
    {
        switch (type) {
            case "Lobby":
                LodingSceneMgr.LoadScene("MainLobby");
                Debug.Log("Loding");
                break;
            case "BattleMode":
                LodingSceneMgr.LoadScene("BattleMode");
                Debug.Log("BattleMode");
                break;
            case "NomalMode":
                LodingSceneMgr.LoadScene("BattleUI");
                Debug.Log("BattleMode");
                break;
        }


    }
}

