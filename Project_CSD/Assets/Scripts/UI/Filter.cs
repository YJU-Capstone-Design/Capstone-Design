using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class Filter : MonoBehaviour
{
    [Header("image")]
    [SerializeField] private GameObject[] bg;
    [SerializeField] private GameObject[] title;
    [SerializeField] private GameObject[] bannel;
    [SerializeField] private GameObject[] filter;
    [SerializeField] private GameObject[] Spellfilter;
    [SerializeField] private TMP_Text filterTxt;
    [SerializeField] private GameObject cardCurrent;
    //미보유 카드 필터
    [SerializeField] private Transform[] unFilter;

    private void Awake()
    {
        Clear();
        filterTxt.text = "캐릭터 카드";
        unFilterCard();
    }
    public void Charclick()
    {
        Clear();
        filterTxt.text = "캐릭터 카드";
        unFilterCard();
    }
    public void Spellclick()
    {
        bg[0].SetActive(false);
        bg[1].SetActive(true);
        title[0].SetActive(false);
        title[1].SetActive(true);
        bannel[0].SetActive(false);
        bannel[1].SetActive(true);
        bannel[2].SetActive(false);
        bannel[3].SetActive(true);
        cardCurrent.SetActive(true);
        foreach (GameObject go in filter)
        {
            go.SetActive(false);
        }
        filter[1].SetActive(true);
        foreach (GameObject go in Spellfilter)
        {
            go.SetActive(false);
        }
        Spellfilter[0].SetActive(true);

        filterTxt.text = "스펠 카드";
        unFilterCard();
    }
    void Clear()
    {
        //배경 초기화
        bg[0].SetActive(true);
        bg[1].SetActive(false);
        cardCurrent.SetActive(false);
        //타이틀 초기화 
        title[0].SetActive(true);
        title[1].SetActive(false);

        //배너 초기화
        bannel[0].SetActive(true);
        bannel[1].SetActive(false);
        bannel[2].SetActive(true);
        bannel[3].SetActive(false);

        //배너 상세 초기화
        foreach(GameObject go in filter)
        {
            go.SetActive(false);
        }
        filter[2].SetActive(true);
        foreach (GameObject go in Spellfilter)
        {
            go.SetActive(false);
        }
        Spellfilter[3].SetActive(true);
    }
























    void unFilterCard()
    {
        GameObject goImage;
        Color color;
        for (int i=0; i < unFilter[0].childCount; i++)
        {
            goImage = unFilter[0].GetChild(i).gameObject.transform.GetChild(2).gameObject;
            color = goImage.GetComponent<Image>().color;
            color.a = 0.6f;
            goImage.GetComponent<Image>().color = color;
        }

        for (int i = 0; i < unFilter[1].childCount; i++)
        {
            goImage = unFilter[1].GetChild(i).gameObject.transform.GetChild(2).gameObject;
            color = goImage.GetComponent<Image>().color;
            color.a = 0.6f;
            goImage.GetComponent<Image>().color = color;
        }

    }
}
