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
        
        if(select_Item == 0)
        {
            select = true;
            //StartCoroutine(Select_Item());
            select_Icon.SetActive(true);
            select_Item = 2;
            Debug.Log("Select ON");
        }
        else if (select_Item == 2)
        {
            select = false;
            //StopCoroutine(Select_Item());
            select_Icon.SetActive(false);

            select_Item = 0;
            Debug.Log("Select OFF");
        }
    }
 

    public void OpenBreadInfo()
    {
        BreakRackMgr.Instance.BreadInfoValue(data);
    }


    IEnumerator Select_Item()
    {
        while (select)
        {
            select_Icon.SetActive(true);
            yield return new WaitForSeconds(0.3f);  // 0.3초 대기
            select_Icon.SetActive(false);
            yield return new WaitForSeconds(0.3f);  // 0.3초 대기
        }
    }

    private void OnDisable()
    {
        select = false;
        StopCoroutine(Select_Item());
    }
}
