using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [SerializeField] Transform creatRanking_Tr; // 랭킹 프리팹 생성 위치
    [SerializeField] GameObject ranking_Obj; // 랭킹 아이템 프리팹
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
        // 데이터베이스에서 1위부터 10위까지의 랭킹 데이터를 가져옴
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
            Debug.Log("랭킹 데이터를 가져오지 못했습니다.");
        }
    }
}
