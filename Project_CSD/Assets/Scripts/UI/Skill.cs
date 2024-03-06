using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : Singleton<Skill>
{
    public static Skill instance;
    public bool skillRange = false;
    public int skillCnt = 0;
    public bool skillStop1 = false;
    public bool uiMenu = false;

    public Image mouse;
    public Sprite normal;
    public Sprite range;

    void Awake(){
        instance=this;
        

    }
    void Update(){

        if(Input.GetMouseButtonDown(0) && skillRange==true){
            mouse.sprite = normal;
            mouse.color = new Color(1,1,1,1f);
            mouse.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            Debug.Log("Range Skill Acivated");
            UiManager.Instance.ReDraw();
            skillRange = false;
            skillStop1 = false;

        }
        if(Input.GetMouseButtonDown(1) && skillRange==true ){
            SkillCancel();
        }
        if(Input.GetKeyDown(KeyCode.Escape) && skillRange==true ){
            SkillCancel();
        }
    }

    public void OnRange(){
        if(skillRange == true && skillStop1 == true){
            Debug.Log("Skill Go2");
            mouse.sprite = range;
            mouse.transform.localScale = new Vector3(6.0f, 6.0f, 1.0f);
            mouse.color = new Color(1,1,1,0.5f);
        }
       
        
       
    }

    public void SkillStay(){
         mouse.sprite = normal;
         mouse.color = new Color(1,1,1,1f);
         mouse.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
         Debug.Log("Skill Stay");
         skillRange = false;
     }

    public void SkillGo(){
        if(uiMenu)
            return;
        Debug.Log("Skill Go");
        skillRange = true;
        OnRange();
     }

    public void SkillCancel(){
        mouse.sprite = normal;
        mouse.color = new Color(1,1,1,1f);
        mouse.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Debug.Log("Range Skill Cancel");
        skillRange = false;
        skillCnt = 0;
        skillStop1 = false;
    }
}
