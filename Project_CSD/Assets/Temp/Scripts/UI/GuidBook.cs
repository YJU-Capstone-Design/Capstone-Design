using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GuidBook : MonoBehaviour
{
    [SerializeField] GameObject guid_Book;// �ֻ��� ������Ʈ
    [SerializeField] Image guid; //�̹����� ������ ������Ʈ
    [SerializeField] GameObject[] guid_Chceck; //� ���̵尡 Ȱ��ȭ ���ִ��� ǥ���ϴ� ��ư
    [SerializeField] Sprite[] guid_Img; // ���̵� �̹���

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
