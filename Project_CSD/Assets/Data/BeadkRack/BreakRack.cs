using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BreakRack : Singleton<BreakRack>
{
    

    [Header("메인 Top오브젝트")]
    [SerializeField] private GameObject topBox;
    [SerializeField] private GameObject itemBox;
    [SerializeField] private GameObject infoBox;
    [SerializeField] private GameObject popUp;

    //결정버튼 누르지 않고 취소했을때 뜨는 팝업창 변수 0=아무런 변경 사항이 없을때 1= 저장버튼을 눌렸을때 2=변경사항이 있지만 저장버튼을 안눌렸을때
    [SerializeField] private int save = 0;

    //전체 적용 능력치
    [SerializeField] private TextMeshProUGUI setInfo;

    private void Awake()
    {
      
        Clear();

    }

    




   



    public void OpenTopBox()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound();}
        topBox.transform.localScale = Vector3.one;
        save = 0;
    }
    public void OpenItemBox()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        itemBox.transform.localScale = Vector3.one;
        infoBox.transform.localScale = Vector3.one;
    }
   
    public void OpenPopUpBox()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        popUp.SetActive(true);
    }

    public void TopMenuCancel()//팝업창이 나오는 경우 -> 설정을 변경후 저장 버튼을 누르지 않을때
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }

        Clear();
        
    }


    public void Cancel(GameObject go)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
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
