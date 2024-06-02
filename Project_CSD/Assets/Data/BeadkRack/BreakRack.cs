using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BreakRack : Singleton<BreakRack>
{
    

    [Header("���� Top������Ʈ")]
    [SerializeField] private GameObject topBox;
    [SerializeField] private GameObject itemBox;
    [SerializeField] private GameObject infoBox;
    [SerializeField] private GameObject popUp;

    //������ư ������ �ʰ� ��������� �ߴ� �˾�â ���� 0=�ƹ��� ���� ������ ������ 1= �����ư�� �������� 2=��������� ������ �����ư�� �ȴ�������
    [SerializeField] private int save = 0;

    //��ü ���� �ɷ�ġ
    [SerializeField] private TextMeshProUGUI setInfo;

    private void Awake()
    {
      
        Clear();

    }

    




   



    public void OpenTopBox()
    {
        topBox.transform.localScale = Vector3.one;
    }
    public void OpenItemBox()
    {
        itemBox.transform.localScale = Vector3.one;
        infoBox.transform.localScale = Vector3.one;
    }
   
    public void OpenPopUpBox()
    {
        popUp.SetActive(true);
    }

    public void TopMenuCancel()//�˾�â�� ������ ��� -> ������ ������ ���� ��ư�� ������ ������
    {
        if (save == 0||save==2)
        {
            popUp.SetActive(true);
            save = 3;

        }else if(save == 3)
        {
          
            Clear();
        }
    }
    public void Show_CaseSave()
    {
        save = 3;
    }

    public void Cancel(GameObject go)
    {
        go.transform.localScale = Vector3.zero;
    }
    public void Clear()
    {
        topBox.transform.localScale= Vector3.zero;
        itemBox.transform.localScale = Vector3.zero;
        infoBox.transform.localScale = Vector3.zero;
        popUp.SetActive(false);
        save = 0;
    }
}
