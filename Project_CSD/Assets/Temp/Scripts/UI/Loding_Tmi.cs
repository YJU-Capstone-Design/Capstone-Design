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
                tmi_Text.text = "Tmi" + "\r\n신묘한 힘으로 인해 탄생한 소라빵 \r\n먹힌다면 꼬리부터 먹히는가 \r\n머리부터 먹히는가 언제나 고민중이다.";
                break;
            case 1:
                tmi_Text.text = "Tmi" + "\r\n신묘한 힘으로 인해 탄생한 도넛 \r\n언제나 뚫러버린 구멍을 바라보며\r\n구멍을 매울수 있는 방법을 생각한다.";
                break;
            case 2:
                tmi_Text.text = "Tmi" + "\r\n신묘한 힘으로 인해 탄생한 크로와상 \r\n크로와상 샌드위치의 유행으로 \r\n 배가 갈라지는걸 두려워한다.";
                break;
            case 3:
                tmi_Text.text = "Tmi" + "\r\n신묘한 힘으로 인해 탄생한 Ramo \r\n배에 들어있는 잼은 \r\n딸기잼일까? 생크림일까?";
                break;
            case 4:
                tmi_Text.text = "Tmi" + "\r\n신묘한 힘으로 인해 탄생한 단팥빵 \r\n가끔씩 튀어나온 팥으로 \r\n수염을 만드는게 취미다.";
                break;
            case 5:
                tmi_Text.text = "Tmi" + "\r\n신묘한 힘으로 인해 탄생한 식빵 \r\n식빵 자투리의 유용성을 증명하기 위해 \r\n 언제나 요리책을 보며 공부한다.";
                break;
        }

    }

}
