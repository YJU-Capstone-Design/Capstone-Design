using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBTest : MonoBehaviour
{
    public PlayerDB[] playerData = new PlayerDB[10];  // 플레이어 데이터 배열 (최대 10명)
    [SerializeField] Transform rankingParent;             // 랭킹 아이템을 생성할 부모 객체
    [SerializeField] GameObject rankItemPrefab;           // 랭킹 아이템 프리팹
    [SerializeField] List<RankItem> rankItems = new List<RankItem>();  // 랭킹 UI 아이템 리스트

    private void Start()
    {
        LoadRankings();  // 랭킹 데이터를 불러와서 초기화
        DisplayRankings(); // 랭킹을 UI에 표시
    }

    // 랭킹 데이터를 불러오는 함수 (여기서는 임의로 데이터를 입력)
    void LoadRankings()
    {
        // 임의 데이터로 랭킹 초기화
        for (int i = 0; i < 10; i++)
        {
            playerData[i] = new PlayerDB
            {
                NAME = "Player " + (i + 1),
                Score = Random.Range(100, 1000)  // 임의 점수
            };
        }

        // 점수 기준으로 오름차순 정렬
        System.Array.Sort(playerData, (x, y) => y.Score.CompareTo(x.Score));  // 내림차순 정렬 (높은 점수가 먼저)
    }

    // 랭킹을 UI에 표시하는 함수
    void DisplayRankings()
    {
        // 기존에 생성된 UI 아이템이 있으면 삭제
        foreach (var item in rankItems)
        {
            Destroy(item.gameObject);
        }
        rankItems.Clear();  // 리스트 초기화

        // 랭킹 아이템 생성
        for (int i = 0; i < playerData.Length; i++)
        {
            GameObject rankItem = Instantiate(rankItemPrefab, rankingParent);
            RankItem rankItemScript = rankItem.GetComponent<RankItem>();
            rankItemScript.SetRankingData(playerData[i].NAME, playerData[i].Score, i + 1); // 순위는 1부터 시작

            rankItems.Add(rankItemScript);  // UI 항목 리스트에 추가
        }
    }
}
