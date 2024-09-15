using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Drawing;
using TMPro;
using System.Xml;
public class BattleManager :Singleton<BattleManager>
{
    public enum BattleState { Start, Win, Lose, BreakTime }
    public BattleState battleState;

    [Header("Shop")]
    [SerializeField] private Transform shopParent;
    [SerializeField] private GameObject[] card;
    public List<GameObject> cardObj = new List<GameObject>();

    [Header("HpBar")] // 메인 집
    public float curHealth; //* 현재 체력
    public float maxHealth; //* 최대 체력
    public GameObject healthBar; // 벽 체력바
    public Slider HpBarSlider;

    [Header("BattleMgr")]
    [SerializeField] private GameObject battle;
    [SerializeField] private GameObject gameEnd;
    [SerializeField] public Transform mainCamera;
    [SerializeField] public int wave;

    [Header("Spawn")]
    public PoolManager pool;
    public Vector3 point; // 마우스 클릭 포인트
    public Transform[] unitSpawnPoint; // 기본 spawn point
    [SerializeField] List<TextAsset> enemySpawnFile; // Enemy Spawn 이 적혀있는 Text File
    [SerializeField] List<Spawn> spawnList; // Text File 에서 읽어들인 값을 저장시키긴 위한 List
    int spawnIndex; // 스폰 로직 순서
    bool spawnEnd; // 스폰 로직 마지막
    float curSpawnTime;
    float nextSpawnDelay;
    public GameObject unitSpawnRange; // 유닛 스폰 범위 (canvas 오브젝트)
    public int totalEnemyCount; // 스폰될 적의 총 수

    [Header("# UI")]
    [SerializeField] GameObject waveUI;
    [SerializeField] Animator[] waveUIChild;
    [SerializeField] GameObject resultUI;
    [SerializeField] Sprite[] waveNumImg;
    [SerializeField] Image[] battleWaveImg;
    [SerializeField] Sprite[] resultImg;
    [SerializeField] Image resultPanel;
    [SerializeField] Image resultMenuBar;
    [SerializeField] Image[] resultWaveImg;
    [SerializeField] Animator[] resultObjsAnim;
    [SerializeField] Button[] resultButtons;
    [SerializeField] TextMeshProUGUI rainkg_Btn_Text;
    [SerializeField] public Button reRoll;
    [SerializeField] TextMeshProUGUI enemyCountText; // 적의 수를 표시할 UI Text
    [SerializeField] Animator reroll_Anim;

    enum UnitType { Bread, Pupnut, Kitchu, Ramo, Sorang, Croirang }; // 테스트(제작)용
    UnitType unitType;

    [Header("# 타이머와 웨이브")]
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI result_Time;
    public float timer = 180;
    private float limite_time = 600f;
    [SerializeField] Image waveImg;
    [SerializeField] Image waveImg2;
    public float endTime=0f;
    bool victory = false;

    [Header("# 랭킹")]
    [SerializeField] GameObject rank_Obj;
    [SerializeField] GameObject rank_Item;
    public TextMeshProUGUI playerScoreText;
    public int playerScore;

    private void Awake()
    {
        // 전투 시작
        battleState = BattleState.Start;
        wave = 0;
        playerScore = 0;
        StartCoroutine(Wave());

        // 전투 기본 세팅
        battle.SetActive(true);
        gameEnd.SetActive(false);
        CardMake();
        if (PlayerData.instance != null) { if (PlayerData.instance.mainHp_Stu >= 1) { maxHealth *= PlayerData.instance.mainHp_Stu; } }
        curHealth = maxHealth;
        UpdateHealthBar();

        spawnList = new List<Spawn>();

        ReadSpawnFile(wave); // 적 유닛 스폰 파일 가져오기

        unitType = UnitType.Bread; // 테스트(제작)용
   
    }

    void Update()
    {
        if (CardManager.Instance.enemys.Count > 0 && curHealth > 0)
        {
            battleState = BattleState.Start;
        }

        if (battleState != BattleState.Start)
            return;

        if (!spawnEnd) { curSpawnTime += Time.deltaTime; } // 스폰이 끝났을 경우 스폰 타임은 증가 X

        // 몬스터 스폰
        if (curSpawnTime > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnTime = 0;
        }

        // 모든 적이 소환된 후, 필드에 남아있는 적이 없고, 벽의 hp가 남아 있을 때 상황 처리
        if (spawnEnd && CardManager.Instance.enemys.Count == 0 && curHealth > 0)
        {
            Debug.Log("End Wave");
            // 모든 enemySpawnFile 을 다 처리했을 경우 Win
            if (wave + 1 == enemySpawnFile.Count)
            {
                battleState = BattleState.Win;
                unitSpawnRange.SetActive(false);
                EndGame("Win");
            }
            // 아직 처리하지 못한 enemySpawnFile 이 남아있을 경우 다음 Wave 실행
            else if (wave + 1 < enemySpawnFile.Count && battleState == BattleState.Start)
            {
                Debug.Log("Next Wave");

                battleState = BattleState.BreakTime;
                wave++;
                ReadSpawnFile(wave); // 적 유닛 스폰 파일 가져오기
                battleState = BattleState.Start;

                StartCoroutine(Wave());
            }
        }

        // 적 처치 후 남은 적의 수 업데이트
        if (!spawnEnd && CardManager.Instance.enemys.Count > 0)
        {
            UpdateEnemyCountUI();
        }

        //Invoke("BattleTimer", 2f);
        BattleTimer();
    }

    void BattleTimer()//타이머
    {
        //클리어까지의 시간

        /*
        limite_time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(limite_time / 60);
        int seconds = Mathf.FloorToInt(limite_time % 60);

        time.text = string.Format("{0:00} : {1:00}", minutes, seconds);*/

        //타임 어택
        limite_time -= Time.deltaTime;
        if (limite_time <= 0)
        {
            battleState = BattleState.Win;
            unitSpawnRange.SetActive(false);
            EndGame("Win");
            //지금 10분을 버텨내면 승리 조건 10분안에 클리어 못하면 패배하는걸로 바꿔야 될까?
        }
        if (limite_time >= 0)
        {
            int minutes = Mathf.FloorToInt(limite_time / 60);
            int seconds = Mathf.FloorToInt(limite_time % 60);

            time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        }


    }

    IEnumerator Wave()
    {
        // Wave UI 활성화
        waveUI.SetActive(true);

        int waveCount = wave + 1;

        // 숫자 이미지 변경
        int ten = waveCount / 10 > 0 ? waveCount / 10 : 0;
        int one = waveCount % 10;

        battleWaveImg[0].sprite = waveNumImg[one];
        battleWaveImg[1].sprite = waveNumImg[ten];
        waveImg.sprite = waveNumImg[ten];
        waveImg2.sprite = waveNumImg[one];
        // Wave 애니메이션
        foreach (Animator waveAnim in waveUIChild)
        {
            waveAnim.SetBool("next", true);
        }

        yield return new WaitForSeconds(2.2f);

        // Wave UI 비활성화
        waveUI.SetActive(false);
    }

    public void EndGame(string whether)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound();}

        resultUI.SetActive(true);

        int waveCount = wave + 1;

        // 숫자 이미지 변경
        int ten = waveCount / 10 > 0 ? waveCount / 10 : 0;
        int one = waveCount % 10;

        // 랭킹 등록 UI 에 Player 점수 최신화
        playerScoreText.text = playerScore.ToString();

        resultWaveImg[0].sprite = waveNumImg[one];
        resultWaveImg[1].sprite = waveNumImg[ten];
        waveImg.sprite = waveNumImg[ten];
        waveImg2.sprite = waveNumImg[one];
        if (whether == "Win")
        {
            resultPanel.sprite = resultImg[0];
            resultMenuBar.sprite = resultImg[2];
            
            if (AudioManager.instance != null) { AudioManager.instance.BattleEndSound(true); }
            if (CashManager.instance != null) { CashManager.instance.player_Gold += 100 * wave; }
            if (PlayerData.instance != null) { PlayerData.instance.Lv++; }
            StartCoroutine(ResultUI(2));
            //endTime = limite_time;
            //int minutes = Mathf.FloorToInt(endTime / 60);
            //int seconds = Mathf.FloorToInt(endTime % 60);
            //result_Time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
            victory = true;
        }
        else if(whether == "Lose")
        {
            resultPanel.sprite = resultImg[1];
            resultMenuBar.sprite = resultImg[3];
            if (AudioManager.instance != null) { AudioManager.instance.BattleEndSound(false); }
            Debug.Log("Lose");
            StartCoroutine(ResultUI(0));
            //endTime = limite_time;
            //int minutes = Mathf.FloorToInt(endTime / 60);
            //int seconds = Mathf.FloorToInt(endTime % 60);
            //result_Time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        }

        // 데이터베이스 입력 (userData Table)
        SaveUserData(whether, waveCount);
    }

    // 게임 후, 데이터베이스에 데이터 입력 버튼 함수 (ranking Table)
    public void SaveUserRanking()
    {
        // 데이터베이스 입력
        XmlNodeList selectedData = DBConnect.Select("ranking", $"WHERE userName = '{UserRankingData.instance.playerName}'");

        if (selectedData == null)
        {
            DBConnect.Insert("ranking", $"{UserRankingData.instance.playerName}, {playerScore}");
        }
        else
        {
            DBConnect.UpdateRanking("ranking", "score", playerScore, $"userName = '{UserRankingData.instance.playerName}'");
        }
    }

    // 게임 종료 후 유저의 게임 결과를 저장하는 함수 (userData Table)
    void SaveUserData(string whether, int wave)
    {
        // 데이터베이스 입력 (userData Table)
        XmlNodeList selectedData = DBConnect.Select("userData", $"WHERE userName = '{UserRankingData.instance.playerName}'");

        if (selectedData == null)
        {
            if (whether == "Win")
            {
                DBConnect.UserDataInsert(UserRankingData.instance.playerName, wave + 1);
            }
            else if (whether == "Lose" && wave >= 2)
            {
                DBConnect.UserDataInsert(UserRankingData.instance.playerName, wave);
            }
        }
        else
        {
            if (whether == "Win")
            {
                DBConnect.UserDataUpdate(UserRankingData.instance.playerName, wave + 1);
            }
            else if (whether == "Lose" && wave >= 2)
            {
                DBConnect.UserDataUpdate(UserRankingData.instance.playerName, wave);
            }
        }
    }

    IEnumerator ResultUI(int second)
    {
        Time.timeScale = 1;

        yield return new WaitForSeconds(second);

        // 애니메이션
        for (int i = 0; i < resultObjsAnim.Length; i++)
        {
            Animator anim = resultObjsAnim[i];
            if ((i == resultObjsAnim.Length - 1 && wave == 0) || (i == resultObjsAnim.Length - 2 && wave == 0))
            {
                // 첫번째 wave인 경우, 랭킹 버튼 오브젝트들은 애니메이션을 적용하지 않음
                continue;
            }
            anim.SetBool("end", true);
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < resultButtons.Length; i++)
        {
            Button resultBtn = resultButtons[i];
            if ((i == resultObjsAnim.Length - 1 && wave == 0) || (i == resultObjsAnim.Length - 2 && wave == 0))
            {
                // 첫번째 wave인 경우, 랭킹 버튼 오브젝트들은 비활성화 상태를 유지함
                continue;
            }
            resultBtn.enabled = true;
        }
    }

    // 유닛 스폰 버튼
    public void UnitSpawn()
    {
        // 마우스 좌클릭 한 곳의 위치값
        point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -Camera.main.transform.position.z));

        switch (unitType)
        {
            case UnitType.Bread:
                pool.Get(0, 0);
                break;
            case UnitType.Pupnut:
                pool.Get(0, 1);
                break;
            case UnitType.Kitchu:
                pool.Get(0, 2);
                break;
            case UnitType.Ramo:
                pool.Get(0, 3);
                break;
            case UnitType.Sorang:
                pool.Get(0, 4);
                break;
            case UnitType.Croirang:
                pool.Get(0, 5);
                break;
        }

        unitSpawnRange.SetActive(false);
    }

    public void HpDamage(float dmg)
    {
        float damage = dmg;
        curHealth -= damage;
        UpdateHealthBar();

    }

    private void CardMake()
    {


        for (int i = 0; i < 3; i++)
        {
            int ran_card = Random.Range(0, card.Length);
            GameObject myInstance;

            myInstance = Instantiate(card[ran_card], shopParent);

            cardObj.Add(myInstance);
        }


    }

    public void CardShuffle(bool Recost)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        
        if (UiManager.Instance.cost >= 1&&Recost)
        {
            reroll_Anim.SetTrigger("ReRoll");
            foreach (GameObject card in cardObj)
            {
                Destroy(card);
            }
            cardObj.Clear();
            CardMake();
            Debug.Log("Shuffle");
            UiManager.Instance.cost -= 1;
        }
        else if (!Recost)
        {
            foreach (GameObject card in cardObj)
            {
                Destroy(card);
            }
            cardObj.Clear();
            CardMake();
            Debug.Log("Shuffle");
        }
        
    }

    void UpdateHealthBar()
    {

        float sliderValue = curHealth / maxHealth;
        HpBarSlider.value = sliderValue;
        if (curHealth <= 0)
        {
            Time.timeScale = 1f;
            if (!resultUI.activeInHierarchy)
            {
                EndGame("Lose"); // 결과창 UI 활성화
            }
            Invoke("Test_GameOver",3f);
        }
    }

    void Test_GameOver()
    {
        healthBar.SetActive(false);
        battle.SetActive(false);
        gameEnd.SetActive(true);
    }
    void ReadSpawnFile(int waveCount)
    {
        // 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // 적의 수 초기화
        totalEnemyCount = 0;

        // 리스폰 파일 읽기
        TextAsset textFile = Resources.Load(enemySpawnFile[waveCount].name) as TextAsset;
        StringReader reader = new StringReader(textFile.text);

        // 한 줄씩 데이터 저장
        while (reader != null)
        {
            string line = reader.ReadLine();

            if (line == null)
                break;

            Spawn spawnData = new Spawn();
            spawnData.spawnDelay = float.Parse(line.Split(',')[0]);
            spawnData.unitType = int.Parse(line.Split(',')[1]);
            spawnData.unitIndex = int.Parse(line.Split(',')[2]);
            spawnData.spawnPoint = int.Parse(line.Split(',')[3]);
            spawnList.Add(spawnData);

            // 적 수 카운트 증가
            totalEnemyCount++;
        }

        // 텍스트 파일 닫기
        reader.Close();

        // 미리 첫 번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].spawnDelay;

        // 적의 수를 UI에 업데이트
        UpdateEnemyCountUI();
    }

    void SpawnEnemy()
    {
        Spawn list = spawnList[spawnIndex];

        PoolManager.Instance.Get(list.unitType, list.unitIndex, list.spawnPoint);

        // 리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            curSpawnTime = 0;
            spawnIndex = 0;
            return;
        }

        // 다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].spawnDelay;
    }

    void UpdateEnemyCountUI()
    {
        enemyCountText.text = totalEnemyCount.ToString();
    }
    public void AddRanking()
    {
        
    }
    public void RankingOpen()
    {
        if (AudioManager.instance != null) { AudioManager.instance.BattleSound(); }
        rank_Obj.SetActive(true);
    }
    public void RankingCloser()
    {
        if (AudioManager.instance != null) { AudioManager.instance.BattleSound(); }
        rank_Obj.SetActive(false);
    }
}
