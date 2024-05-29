using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadRack_Item : MonoBehaviour
{
    [SerializeField] private GameObject select_Icon;
    public bool select = false;
    private void OnEnable()
    {
        select = true;
        StartCoroutine(Select_Item());
    }

    public void Click_Info()
    {
        BreakRack.Instance.OpenInfoBox();
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
