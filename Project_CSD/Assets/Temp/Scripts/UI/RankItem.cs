using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour
{
    [SerializeField] Image rankImg; // 앞자리 이미지
    [SerializeField] Image rankImg2; // 뒷자리 이미지
    [SerializeField] Sprite[] rankImg_No; // 숫자 스프라이트 배열
    [SerializeField] TextMeshProUGUI player_Name; // 플레이어 이름 텍스트
    [SerializeField] TextMeshProUGUI play_Time; // 플레이어 시간 텍스트
    [SerializeField] TextMeshProUGUI player_No; // 플레이어 번호 텍스트

    public void SetRankingData(string name, int score)
    {
        // Rank를 2자리 숫자로 분리하여 이미지에 설정
        //rankImg.sprite = rankImg_No[rank / 10]; // 앞자리
        //rankImg2.sprite = rankImg_No[rank % 10]; // 뒷자리

        // 나머지 데이터 설정
        player_Name.text = name;
        //player_No.text = id.ToString();

        // 일단 테스트(임시) 로 형태는 그대로 놔둠.
        int minutes = Mathf.FloorToInt(score / 60);
        int seconds = Mathf.FloorToInt(score % 60);

        play_Time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
