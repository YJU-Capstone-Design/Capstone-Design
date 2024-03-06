using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public GameObject pop;
    // Update is called once per frame
    void Update()
    {
        
    }
    public void PopupShow(){
        pop.gameObject.SetActive(true);
        Skill.instance.SkillStay();
        Skill.instance.uiMenu = true;
        Debug.Log("StopMenu");
        Show();
    }

    public void Show()
    {

        UiManager.Instance.Stop();
    }

    public void Hide()
    {
        pop.gameObject.SetActive(false);
        Skill.instance.uiMenu = false;
        UiManager.Instance.Resume();
    }
}
