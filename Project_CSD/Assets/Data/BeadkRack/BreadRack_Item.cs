using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BreadRack_Item : MonoBehaviour
{
    [SerializeField] private GameObject select_Icon;
    [SerializeField] private Image icon_Img;
    public BreadRack_Data data;
    public bool select = false;
    public int select_Item = 0;

    private void OnEnable()
    {
        icon_Img.sprite = data.BreadRack_Img;
    }
    public void Select_SetItem()
    {
        
        if(select_Item == 0 && BreakRackMgr.Instance.set_item_Value!=8)
        {
            select = true;
            //StartCoroutine(Select_Item());
            select_Icon.SetActive(true);
            select_Item = 2;
            OpenBreadInfo();
            Debug.Log("Select ON");
        }
        else if (select_Item == 2)
        {
            select = false;
            //StopCoroutine(Select_Item());
            select_Icon.SetActive(false);

            select_Item = 0;
            BreakRackMgr.Instance.Cancel_SetItem(data);
            Debug.Log("Select OFF");
        }
    }
 

    public void OpenBreadInfo()
    {
        BreakRackMgr.Instance.BreadInfoValue(data);
    }


}
