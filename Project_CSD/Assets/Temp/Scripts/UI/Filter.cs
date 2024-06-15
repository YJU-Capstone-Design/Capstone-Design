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
    //보유카드 필터 해제
    [SerializeField] private Transform[] holdfilter;

    [Header("GachaResult")]//가챠 결과 연동
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
        filterTxt.text = "캐릭터 카드";
        unFilterCard();
        
    }
    public void Charclick()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        Clear();
        filterTxt.text = "캐릭터 카드";
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
        // 세팅 전 모든 표시 UI 아이템 off
        for (int i = 0; i < haveItems.Count; i++)
        {
            haveItems[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < noneItems.Count; i++)
        {
            noneItems[i].gameObject.SetActive(false);
        }


        // Todo : 보유한 카드 표시
        for (int i = 0; i < HoldingList.single.holding_Unit.Count; i++)
        {
            getNewCollectionItem(ref haveItems).Init(HoldingList.single.holding_Unit[i]);
        }
        // todo : 미 보유한 카드 표시
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
