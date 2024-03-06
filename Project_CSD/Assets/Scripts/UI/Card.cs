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
                
                Skill.instance.skillRange = true;
                Skill.instance.skillStop1 = true;
                Skill.instance.OnRange();
                Debug.Log("RANGE SKILL ATTACK");
                //GameManager.instance.ReDraw();
                break;
            case "AttackDamage":
                Debug.Log("AttackDamage");
                UiManager.instance.ReDraw();
                break;
            case "AttackSpeed":
                Debug.Log("AttackSpeed");
                UiManager.instance.ReDraw();
                break;
            case "UnitSpeed":
                Debug.Log("UnitSpeed");
                UiManager.instance.ReDraw();
                break;
            case "Defense":
                Debug.Log("Defense");
                UiManager.instance.ReDraw();
                break;
            case "Critical":
                Debug.Log("Critical");
                UiManager.instance.ReDraw();
                break;
            case "BossAttack":
                Debug.Log("BossAttack");
                UiManager.instance.ReDraw();
                break;
            case "UnitMaxHealth":
                Debug.Log("UnitMaxHealth");
                UiManager.instance.ReDraw();
                break;

            case "Heal":   
                Debug.Log("Heal");
                UiManager.instance.ReDraw();
                break;
        }

    }
    


}
