using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Skill.instance.SkillStay();
        Debug.Log("Stay");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Skill.instance.SkillGo();
        Debug.Log("Exit");
    }

    public void SelectCancel(){
        Skill.instance.SkillCancel();
    }

  

    public void GameContunue(){
        Skill.instance.SkillGo();
        Debug.Log("GameReStart");
    }
    

    
   
}
