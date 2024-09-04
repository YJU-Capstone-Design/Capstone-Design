using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [SerializeField] Transform creatRanking_Tr; // ��ŷ ������ ���� ��ġ
    [SerializeField] GameObject ranking_Obj; // ��ŷ ������ ������

    float time;

    private void Start()
    {
        RankingSystem();

    }
    private void Update()
    {
        time += Time.deltaTime;
        if(time >= 180)
        {
            foreach (Transform child in creatRanking_Tr)
            {
                Destroy(child.gameObject);
            }
            RankingSystem();
            time = 0f;
        }
    }

    void RankingSystem()
    {
        // �����ͺ��̽����� 1������ 10�������� ��ŷ �����͸� ������
        XmlNodeList rankingData = DBConnect.Select("ranking", "ORDER BY time ASC LIMIT 10");
        
        if (rankingData != null)
        {
            int rank = 1;
            foreach (XmlNode data in rankingData)
            {
                // �������� �����ϰ� RankItem ��ũ��Ʈ�� ���� ������ ����
                GameObject newRankingObj = Instantiate(ranking_Obj, creatRanking_Tr);
                RankItem rankItem = newRankingObj.GetComponent<RankItem>();

                // XML �����͸� RankItem�� ����
                rankItem.SetRankingData(rank, data["name"].InnerText, int.Parse(data["time"].InnerText), int.Parse(data["id"].InnerText));

                rank++;
            }
        }
        else
        {
            Debug.Log("��ŷ �����͸� �������� ���߽��ϴ�.");
        }
    }
}
