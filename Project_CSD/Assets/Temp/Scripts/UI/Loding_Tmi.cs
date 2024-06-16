using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Loding_Tmi : MonoBehaviour
{
    [SerializeField] private Sprite[] tmi_Img;
    [SerializeField] private Image tmi_BG;
    [SerializeField] private TextMeshProUGUI tmi_Text;
    private void Awake()
    {
        
        int ran = Random.Range(0, tmi_Img.Length);

        tmi_BG.sprite = tmi_Img[ran];
        Tmi(ran);
        
    }
    public void Tmi(int type)
    {
        switch (type) {
            case 0:
                tmi_Text.text = "Tmi" + "\r\n�Ź��� ������ ���� ź���� �Ҷ� \r\n�����ٸ� �������� �����°� \r\n�Ӹ����� �����°� ������ ������̴�.";
                break;
            case 1:
                tmi_Text.text = "Tmi" + "\r\n�Ź��� ������ ���� ź���� ���� \r\n������ �շ����� ������ �ٶ󺸸�\r\n������ �ſ�� �ִ� ����� �����Ѵ�.";
                break;
            case 2:
                tmi_Text.text = "Tmi" + "\r\n�Ź��� ������ ���� ź���� ũ�οͻ� \r\nũ�οͻ� ������ġ�� �������� \r\n �谡 �������°� �η����Ѵ�.";
                break;
            case 3:
                tmi_Text.text = "Tmi" + "\r\n�Ź��� ������ ���� ź���� Ramo \r\n�迡 ����ִ� ���� \r\n�������ϱ�? ��ũ���ϱ�?";
                break;
            case 4:
                tmi_Text.text = "Tmi" + "\r\n�Ź��� ������ ���� ź���� ���ϻ� \r\n������ Ƣ��� ������ \r\n������ ����°� ��̴�.";
                break;
            case 5:
                tmi_Text.text = "Tmi" + "\r\n�Ź��� ������ ���� ź���� �Ļ� \r\n�Ļ� �������� ���뼺�� �����ϱ� ���� \r\n ������ �丮å�� ���� �����Ѵ�.";
                break;
        }

    }

}
