using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [SerializeField] Transform creatRanking_Tr; // ��ŷ ������ ���� ��ġ
    [SerializeField] GameObject ranking_Obj; // ��ŷ ������ ������
    [SerializeField] RankItem[] rankItems;

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
        XmlNodeList rankingData = DBConnect.Select("ranking", "ORDER BY score DESC LIMIT 10");
        
        if (rankingData != null)
        {
            int rank = 0;

            foreach (XmlNode data in rankingData)
            {
                rankItems[rank].SetRankingData(data["userName"].InnerText, int.Parse(data["score"].InnerText));
                rank++;
            }
        }
        else
        {
            Debug.Log("��ŷ �����͸� �������� ���߽��ϴ�.");
        }
    }
}
