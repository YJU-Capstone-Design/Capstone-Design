using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class Filter : MonoBehaviour
{
    [SerializeField] private GameObject[] filter;
    [SerializeField] private TMP_Text filterTxt;

    //미보유 카드 필터
    [SerializeField] private Transform[] unFilter;

    private void Awake()
    {
        filter[0].SetActive(false);
        filter[1].SetActive(false);
        filterTxt.text = "캐릭터 카드";
        unFilterCard();
    }
    public void Charclick()
    {
        filter[0].SetActive(false);
        filter[1].SetActive(false);
        filterTxt.text = "캐릭터 카드";
        unFilterCard();
    }
    public void Spellclick()
    {
        filter[0].SetActive(true);
        filter[1].SetActive(true);
        filterTxt.text = "스펠 카드";
        unFilterCard();
    }

    void unFilterCard()
    {
        GameObject goImage;
        Color color;
        for (int i=0; i < unFilter[0].childCount; i++)
        {
            goImage = unFilter[0].GetChild(i).gameObject.transform.GetChild(2).gameObject;
            color = goImage.GetComponent<Image>().color;
            color.a = 0.3f;
            goImage.GetComponent<Image>().color = color;
        }

        for (int i = 0; i < unFilter[1].childCount; i++)
        {
            goImage = unFilter[1].GetChild(i).gameObject.transform.GetChild(2).gameObject;
            color = goImage.GetComponent<Image>().color;
            color.a = 0.3f;
            goImage.GetComponent<Image>().color = color;
        }

    }
}
