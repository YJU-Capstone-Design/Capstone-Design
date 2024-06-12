using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionSpell : MonoBehaviour
{
    [SerializeField] List<Collection_Data> haveItems;
    [SerializeField] List<Collection_Data> noneItems;
    [SerializeField] Transform parentTr;
    public void OnEnable()
    {

        UpdateCollection();

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

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)parentTr);

        Debug.Log("call collection");

    
    }
}
