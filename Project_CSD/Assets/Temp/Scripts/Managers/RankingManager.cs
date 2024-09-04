using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [SerializeField] Transform creatRanking_Tr; // 랭킹 프리팹 생성 위치
    [SerializeField] GameObject ranking_Obj; // 랭킹 아이템 프리팹
    
    private void Start()
    {
      
        // 데이터베이스에서 1위부터 10위까지의 랭킹 데이터를 가져옴
        XmlNodeList rankingData = DBConnect.Instance.Select("ranking", "ORDER BY time ASC LIMIT 10");

        if (rankingData != null)
        {
            int rank = 1;
            foreach (XmlNode data in rankingData)
            {
                // 프리팹을 생성하고 RankItem 스크립트를 통해 데이터 설정
                GameObject newRankingObj = Instantiate(ranking_Obj, creatRanking_Tr);
                RankItem rankItem = newRankingObj.GetComponent<RankItem>();

                // XML 데이터를 RankItem에 설정
                rankItem.SetRankingData(rank, data["name"].InnerText, int.Parse(data["time"].InnerText), int.Parse(data["id"].InnerText));

                rank++;
            }
        }
        else
        {
            Debug.Log("랭킹 데이터를 가져오지 못했습니다.");
        }
    }
}
