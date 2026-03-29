using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Drawing;
using TMPro;
// using System.Xml; // XML 관련 사용하지 않으므로 주석 처리
using System;

public class BattleManager : Singleton<BattleManager>
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
    [SerializeField] TextMeshProUGUI text_Health;

    [Header("BattleMgr")]
    [SerializeField] private GameObject battle;
    [SerializeField] private GameObject gameEnd;
    [SerializeField] public Transform mainCamera;
    [SerializeField] public int wave;

    [Header("Spawn")]
    public PoolManager pool;
    public Vector3 point; // 터치 / 클릭 포인트
    public Transform[] unitSpawnPoint; // 기본 spawn point
    [SerializeField] List<TextAsset> enemySpawnFile; // Enemy Spawn 이 적혀있는 Text File
    [SerializeField] List<Spawn> spawnList; // Text File 에서 읽어들인 값을 저장시키긴 위한 List
    int spawnIndex; // 스폰 로직 순서
    bool spawnEnd; // 스폰 로직 마지막
    float curSpawnTime;
    float nextSpawnDelay;
    public GameObject unitSpawnRange; // 유닛 스폰 범위 (canvas 오브젝트)
    public int totalEnemyCount; // 스폰될 적의 총 수
    private int enemyCnt;

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
    public float limite_time = 900f;
    [SerializeField] Image waveImg;
    [SerializeField] Image waveImg2;
    public float endTime = 0f;
    bool victory = false;

    [Header("# 랭킹")]
    [SerializeField] GameObject rank_Obj;
    [SerializeField] GameObject rank_Item;
    public TextMeshProUGUI playerScoreText;
    public int playerScore;
    [SerializeField] TextMeshProUGUI percentageText;

    [Header("# percentData")]
    public float percent = 0.0f;
    public float allUserCount = 0;
    public float selectUserCount = 0;


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
        text_Health.text = maxHealth + " / " + maxHealth.ToString();
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

        if (!spawnEnd) { curSpawnTime += Time.deltaTime; }

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
            if (wave + 1 == enemySpawnFile.Count)
            {
                battleState = BattleState.Win;
                unitSpawnRange.SetActive(false);
                EndGame("Win");
            }
            else if (wave + 1 < enemySpawnFile.Count && battleState == BattleState.Start)
            {
                Debug.Log("Next Wave");

                battleState = BattleState.BreakTime;
                wave++;
                ReadSpawnFile(wave);
                battleState = BattleState.Start;

                StartCoroutine(Wave());
            }
        }

        if (CardManager.Instance.enemys.Count >= 0)
        {
            UpdateEnemyCountUI();
        }

        BattleTimer();
    }

    void BattleTimer()
    {
        limite_time -= Time.deltaTime;
        if (limite_time <= 0)
        {
            battleState = BattleState.Lose;
            unitSpawnRange.SetActive(false);
            HpDamage(curHealth);
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
        waveUI.SetActive(true);

        int waveCount = wave + 1;

        int ten = waveCount / 10 > 0 ? waveCount / 10 : 0;
        int one = waveCount % 10;

        battleWaveImg[0].sprite = waveNumImg[one];
        battleWaveImg[1].sprite = waveNumImg[ten];
        waveImg.sprite = waveNumImg[ten];
        waveImg2.sprite = waveNumImg[one];

        foreach (Animator waveAnim in waveUIChild)
        {
            waveAnim.SetBool("next", true);
        }

        yield return new WaitForSeconds(2.2f);

        waveUI.SetActive(false);
    }

    public void EndGame(string whether)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }

        resultUI.SetActive(true);

        int waveCount = wave + 1;

        int ten = waveCount / 10 > 0 ? waveCount / 10 : 0;
        int one = waveCount % 10;

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
            victory = true;
            StartCoroutine(ResultUI(2));
            Invoke("Stop_Anim", 5f);
        }
        else if (whether == "Lose")
        {
            resultPanel.sprite = resultImg[1];
            resultMenuBar.sprite = resultImg[3];
            if (AudioManager.instance != null) { AudioManager.instance.BattleEndSound(false); }
            Debug.Log("Lose");
            StartCoroutine(ResultUI(0));
            Invoke("Stop_Anim", 5f);
        }

        Debug.Log("Get Wave Reach Percentage");

        if (whether == "Win" && waveCount == 10)
        {
            // GetWaveReachPercentage(waveCount + 1);
        }
        else
        {
            // GetWaveReachPercentage(waveCount);
        }
    }

    public void Stop_Anim()
    {
        AnimationController.instance.StopAllAnimations();
    }

    public void SaveUserRanking()
    {
        if (playerScore <= 0)
            return;

        rank_Obj.SetActive(false);
    }

    void SaveUserData(string whether, int wave)
    {
        // 주석 처리된 DB 로직 유지
    }

    IEnumerator ResultUI(int second)
    {
        Time.timeScale = 1;

        yield return new WaitForSeconds(second);

        for (int i = 0; i < resultObjsAnim.Length; i++)
        {
            Animator anim = resultObjsAnim[i];
            anim.SetBool("end", true);
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < resultButtons.Length; i++)
        {
            Button resultBtn = resultButtons[i];
            resultBtn.enabled = true;
        }

        yield return new WaitForSeconds(1);
    }

    // ────────────────────────────────────────────
    // 유닛 스폰 - 터치 / 마우스 자동 분기
    // ────────────────────────────────────────────
    public void UnitSpawn()
    {
        // 터치 지원 기기면 터치 위치, 아니면 마우스 위치 사용
        Vector2 inputPos = (Input.touchSupported && Input.touchCount > 0)
            ? Input.GetTouch(0).position
            : (Vector2)Input.mousePosition;

        point = Camera.main.ScreenToWorldPoint(
            new Vector3(inputPos.x, inputPos.y, -Camera.main.transform.position.z));

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
        text_Health.text = curHealth.ToString() + " / " + maxHealth;
        UpdateHealthBar();
    }

    private void CardMake()
    {
        for (int i = 0; i < 3; i++)
        {
            int ran_card = UnityEngine.Random.Range(0, card.Length);
            GameObject myInstance = Instantiate(card[ran_card], shopParent);
            cardObj.Add(myInstance);
        }

        // 카드 생성 후 RoundUI 강제 리셋
        RoundUI roundUI = shopParent.GetComponent<RoundUI>();
        if (roundUI != null)
            roundUI.ReActivate();
    }

    public void CardShuffle(bool Recost)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }

        if (UiManager.Instance.cost >= 1 && Recost)
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
                EndGame("Lose");
            }
            Invoke("Test_GameOver", 3f);
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
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        totalEnemyCount = 0;
        enemyCnt = 0;

        TextAsset textFile = Resources.Load(enemySpawnFile[waveCount].name) as TextAsset;
        StringReader reader = new StringReader(textFile.text);

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

            totalEnemyCount++;
            enemyCnt++;
        }

        reader.Close();

        nextSpawnDelay = spawnList[0].spawnDelay;

        UpdateEnemyCountUI();
    }

    void SpawnEnemy()
    {
        Spawn list = spawnList[spawnIndex];

        PoolManager.Instance.Get(list.unitType, list.unitIndex, list.spawnPoint);

        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            curSpawnTime = 0;
            spawnIndex = 0;
            return;
        }

        nextSpawnDelay = spawnList[spawnIndex].spawnDelay;
    }

    void UpdateEnemyCountUI()
    {
        enemyCountText.text = totalEnemyCount.ToString() + "/" + enemyCnt;
    }

    public void AddRanking() { }

    public void RankingOpen()
    {
        if (AudioManager.instance != null) { AudioManager.instance.BattleSound(); }
    }

    public void RankingCloser()
    {
        if (AudioManager.instance != null) { AudioManager.instance.BattleSound(); }
    }

    void GetWaveReachPercentage(int wave)
    {
        // DB 연동 주석 처리 유지
    }
}