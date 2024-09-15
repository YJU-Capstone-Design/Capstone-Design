using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameText; // �÷��̾� �̸� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI playerScoreText; // �÷��̾� �ð� �ؽ�Ʈ

    public void SetRankingData(string name, int score)
    {
        // Rank�� 2�ڸ� ���ڷ� �и��Ͽ� �̹����� ����
        //rankImg.sprite = rankImg_No[rank / 10]; // ���ڸ�
        //rankImg2.sprite = rankImg_No[rank % 10]; // ���ڸ�

        // ������ ������ ����
        playerNameText.text = name;
        playerScoreText.text = score.ToString();
    }
}
