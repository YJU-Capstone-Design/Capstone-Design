using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Card : MonoBehaviour
{
    public static Card instance;
    public CardData card;

    
    Image img;
    void Awake(){
        instance = this;
       
        img = GetComponent<Image>();
        //Debug.Log(card.cardType);
        
        CardDraw();
    }

    public void CardDraw(){
        img.sprite= card.cardSprite;
        
    }
    public void BlackCard(){

        img.color = new Color(0,0,0);
        GetComponent<Button>().interactable = false;
    }
  
    public void SkillUsing(string name){
        //string type = card.cardType.ToString();
        switch(name){
            case "RangeAttack":
                
                Skill.Instance.skillRange = true;
                Skill.Instance.skillStop1 = true;
                Skill.Instance.OnRange();
                Debug.Log("RANGE SKILL ATTACK");
                //GameManager.instance.ReDraw();
                break;
            case "AttackDamage":
                Debug.Log("AttackDamage");
                UiManager.Instance.ReDraw();
                break;
            case "AttackSpeed":
                Debug.Log("AttackSpeed");
                UiManager.Instance.ReDraw();
                break;
            case "UnitSpeed":
                Debug.Log("UnitSpeed");
                UiManager.Instance.ReDraw();
                break;
            case "Defense":
                Debug.Log("Defense");
                UiManager.Instance.ReDraw();
                break;
            case "Critical":
                Debug.Log("Critical");
                UiManager.Instance.ReDraw();
                break;
            case "BossAttack":
                Debug.Log("BossAttack");
                UiManager.Instance.ReDraw();
                break;
            case "UnitMaxHealth":
                Debug.Log("UnitMaxHealth");
                UiManager.Instance.ReDraw();
                break;

            case "Heal":   
                Debug.Log("Heal");
                UiManager.Instance.ReDraw();
                break;
        }

    }
    


}
