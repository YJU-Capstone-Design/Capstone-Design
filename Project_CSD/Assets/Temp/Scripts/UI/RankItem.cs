using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour
{
    [SerializeField] Image rankImg; // ���ڸ� �̹���
    [SerializeField] Image rankImg2; // ���ڸ� �̹���
    [SerializeField] Sprite[] rankImg_No; // ���� ��������Ʈ �迭
    [SerializeField] TextMeshProUGUI player_Name; // �÷��̾� �̸� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI play_Time; // �÷��̾� �ð� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI player_No; // �÷��̾� ��ȣ �ؽ�Ʈ

    public void SetRankingData(string name, int score)
    {
        // Rank�� 2�ڸ� ���ڷ� �и��Ͽ� �̹����� ����
        //rankImg.sprite = rankImg_No[rank / 10]; // ���ڸ�
        //rankImg2.sprite = rankImg_No[rank % 10]; // ���ڸ�

        // ������ ������ ����
        player_Name.text = name;
        //player_No.text = id.ToString();

        // �ϴ� �׽�Ʈ(�ӽ�) �� ���´� �״�� ����.
        int minutes = Mathf.FloorToInt(score / 60);
        int seconds = Mathf.FloorToInt(score % 60);

        play_Time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
