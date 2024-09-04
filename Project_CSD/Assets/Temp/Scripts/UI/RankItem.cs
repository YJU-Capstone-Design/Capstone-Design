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

    public void SetRankingData(int rank, string name, int time, int id)
    {
        // Rank�� 2�ڸ� ���ڷ� �и��Ͽ� �̹����� ����
        rankImg.sprite = rankImg_No[rank / 10]; // ���ڸ�
        rankImg2.sprite = rankImg_No[rank % 10]; // ���ڸ�

        // ������ ������ ����
        player_Name.text = name;
        player_No.text = id.ToString();

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        play_Time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
