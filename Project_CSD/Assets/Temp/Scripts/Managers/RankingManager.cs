using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class RankEntry
{
    public string name;
    public int score;
}

[System.Serializable]
public class RankData
{
    public List<RankEntry> entries = new List<RankEntry>();
}

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    [SerializeField] RankItem[] rankItems;

    private const string FILE_NAME = "ranking.json";
    private const int MAX_RANK = 10;
    private RankData rankData = new RankData();
    private string FilePath => Path.Combine(Application.persistentDataPath, FILE_NAME);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadRanking();
        RankingSystem();
    }

    // 랭킹 UI 갱신
    public void RankingSystem()
    {
        for (int i = 0; i < rankItems.Length; i++)
        {
            if (i < rankData.entries.Count)
                rankItems[i].SetRankingData(rankData.entries[i].name, rankData.entries[i].score);
            else
                rankItems[i].SetRankingData("-", 0); // 빈 슬롯
        }
    }

    // 10위 안에 드는 점수인지 확인
    public bool IsRankable(int score)
    {
        if (rankData.entries.Count < MAX_RANK) return true;
        return score > rankData.entries[MAX_RANK - 1].score;
    }

    // 진열대 쓰면 붙음
    private string ProcessPlayerName(string playerName)
    {
        if (PlayerData.instance == null) return playerName;

        float atk = PlayerData.instance.atk_Stu;
        float speed = PlayerData.instance.speed_Stu;
        float mainHp = PlayerData.instance.mainHp_Stu;
        float unitHp = PlayerData.instance.unitHp_Stu;

        float total = atk + speed + mainHp + unitHp;
        bool allOne = (atk == 1f && speed == 1f && mainHp == 1f && unitHp == 1f);
        bool allZero = (total == 0f);

        if (allZero || allOne)
            return playerName;
        else
            return playerName + "(허접)";
    }

    // 기존 AddRanking
    public void AddRanking(string playerName, int score)
    {
        string finalName = ProcessPlayerName(playerName); // 이름 가공

        rankData.entries.Add(new RankEntry { name = finalName, score = score });
        rankData.entries.Sort((a, b) => b.score.CompareTo(a.score));
        if (rankData.entries.Count > MAX_RANK)
            rankData.entries.RemoveRange(MAX_RANK, rankData.entries.Count - MAX_RANK);
        SaveRanking();
        RankingSystem();
    }

    private void SaveRanking()
    {
        string json = JsonUtility.ToJson(rankData, true);
        File.WriteAllText(FilePath, json);
    }

    private void LoadRanking()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            rankData = JsonUtility.FromJson<RankData>(json);
        }
    }
}