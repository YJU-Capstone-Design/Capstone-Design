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

    //�̺��� ī�� ����
    [SerializeField] private Transform[] unFilter;

    private void Awake()
    {
        filter[0].SetActive(false);
        filter[1].SetActive(false);
        filterTxt.text = "ĳ���� ī��";
        unFilterCard();
    }
    public void Charclick()
    {
        filter[0].SetActive(false);
        filter[1].SetActive(false);
        filterTxt.text = "ĳ���� ī��";
        unFilterCard();
    }
    public void Spellclick()
    {
        filter[0].SetActive(true);
        filter[1].SetActive(true);
        filterTxt.text = "���� ī��";
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
