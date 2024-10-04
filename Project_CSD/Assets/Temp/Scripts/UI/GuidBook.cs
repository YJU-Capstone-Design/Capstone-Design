using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GuidBook : MonoBehaviour
{
    [SerializeField] GameObject guid_Book;// 최상위 오브젝트
    [SerializeField] Image guid; //이미지를 변경할 오브젝트
    [SerializeField] GameObject[] guid_Chceck; //어떤 가이드가 활성화 돼있는지 표시하는 버튼
    [SerializeField] Sprite[] guid_Img; // 가이드 이미지

    private void Start()
    {
        guid_Book.SetActive(false);
        GuidClear();
    }
    public void Guid(int page)
    {
        guid.sprite = guid_Img[page];
     
        for (int i = 0; i < guid_Chceck.Length; i++)
        {
            if (i != page)
            {
                guid_Chceck[i].SetActive(false);
            }
            else
            {
                guid_Chceck[i].SetActive(true);
            }
          
        }
    }

    public void GuidClear()
    {
        guid.sprite = guid_Img[0];
        guid_Chceck[0].SetActive(true);
        for(int i=1; i< guid_Chceck.Length; i++)
        {
            guid_Chceck[i].SetActive(false);
        }
    }
    public void Open_GuidBook()
    {
        GuidClear();
        guid_Book.SetActive(true);
    }
    public void Close_GuidBook()
    {
        GuidClear();
        guid_Book.SetActive(false);

    }
}
