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
    //�̺��� ī�� ����
    [SerializeField] private Transform[] unFilter;
    //����ī�� ���� ����
    [SerializeField] private Transform[] holdfilter;

    [Header("GachaResult")]//��í ��� ����
    [SerializeField] Transform unitTr;
    [SerializeField] Transform unitTr_Disable;
    [SerializeField] List<Collection_Data> haveItems;
    [SerializeField] List<Collection_Data> noneItems;
    public HoldingList holdingList;

    private void Awake()
    {
     
    }
    private void Start()
    {
      
        Clear();
        filterTxt.text = "ĳ���� ī��";
        unFilterCard();
        
    }
    public void Charclick()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        Clear();
        filterTxt.text = "ĳ���� ī��";
        unFilterCard();
      
    }
    public void Spellclick()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
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

        filterTxt.text = "���� ī��";
        unFilterCard();

    }
    void Clear()
    {
        //��� �ʱ�ȭ
        bg[0].SetActive(true);
        bg[1].SetActive(false);
        cardCurrent.SetActive(false);
        //Ÿ��Ʋ �ʱ�ȭ 
        title[0].SetActive(true);
        title[1].SetActive(false);

        //��� �ʱ�ȭ
        bannel[0].SetActive(true);
        bannel[1].SetActive(false);
        bannel[2].SetActive(true);
        bannel[3].SetActive(false);

        //��� �� �ʱ�ȭ
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






    public void OnEnable()
    {
        
        UpdateCollection();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)unitTr.parent);
    }

    private Collection_Data getNewCollectionItem(ref List<Collection_Data> baseList)
    {
        for (int i = 0; i < baseList.Count; i++)
        {
            if (!baseList[i].gameObject.activeSelf)
                return baseList[i];
        }

        Collection_Data temp = Instantiate(baseList[0], baseList[0].transform.position, Quaternion.identity);
        temp.transform.SetParent(baseList[0].transform.parent);
        baseList.Add(temp);
        return temp;
    }
    public void UpdateCollection()
    {
        // ���� �� ��� ǥ�� UI ������ off
        for (int i = 0; i < haveItems.Count; i++)
        {
            haveItems[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < noneItems.Count; i++)
        {
            noneItems[i].gameObject.SetActive(false);
        }


        // Todo : ������ ī�� ǥ��
        for (int i = 0; i < HoldingList.single.holding_Unit.Count; i++)
        {
            getNewCollectionItem(ref haveItems).Init(HoldingList.single.holding_Unit[i]);
        }
        // todo : �� ������ ī�� ǥ��
        for (int i = 0; i < GachaManager.single.listGachaTemplete.Count; i++)
        {
            UnitData have = HoldingList.single.holding_Unit.Find(val => val.UnitID == GachaManager.single.listGachaTemplete[i].UnitID);
            if (have != null)
                continue;
            getNewCollectionItem(ref noneItems).Init(GachaManager.single.listGachaTemplete[i]);
        }

        //LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)unitTr.parent);
        StartCoroutine(RebuildAtEndOfFrame((RectTransform)unitTr.parent));
        Debug.Log("call collection");

        //HoldFilterCard();
        
    }



    IEnumerator RebuildAtEndOfFrame(RectTransform rectTransform)
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }






    void HoldFilterCard()
    {
        GameObject goImage;
        Color color;
        for (int i = 0; i < holdfilter[0].childCount; i++)
        {
            goImage = holdfilter[0].GetChild(i).gameObject.transform.GetChild(2).gameObject;
            color = goImage.GetComponent<Image>().color;
            color.a = 0f;
            goImage.GetComponent<Image>().color = color;
        }

        for (int i = 0; i < holdfilter[1].childCount; i++)
        {
            goImage = holdfilter[1].GetChild(i).gameObject.transform.GetChild(2).gameObject;
            color = goImage.GetComponent<Image>().color;
            color.a = 0f;
            goImage.GetComponent<Image>().color = color;
        }

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
