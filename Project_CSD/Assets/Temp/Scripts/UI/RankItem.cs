// 기존 코드 그대로 사용 가능, 빈 슬롯 처리만 추가
using TMPro;
using UnityEngine;

public class RankItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI playerScoreText;

    public void SetRankingData(string name, int score)
    {
        playerNameText.text = name;
        playerScoreText.text = score == 0 ? "-" : score.ToString();
    }
}