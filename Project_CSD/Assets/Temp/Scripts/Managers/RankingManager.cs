using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RankingManager : MonoBehaviour
{
    [SerializeField] Transform creatRanking_Tr; // ��ŷ ������ ���� ��ġ
    [SerializeField] GameObject ranking_Obj; // ��ŷ ������ ������
    [SerializeField] RankItem[] rankItems;
    [SerializeField] GameObject noNetwork;
    float time;

    private void Start()
    {
        RankingSystem();

    }
    private void Update()
    {
        time += Time.deltaTime;
        if(time >= 60)
        {
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
            ranking_Obj.SetActive(true);
            noNetwork.SetActive(false);
            foreach (XmlNode data in rankingData)
            {
                rankItems[rank].SetRankingData(data["userName"].InnerText, int.Parse(data["score"].InnerText));
                rank++;
            }
        }
        else
        {
            Debug.Log("��ŷ �����͸� �������� ���߽��ϴ�.");
            noNetwork.SetActive(true);
            ranking_Obj.SetActive(false);
        }
    }
}
