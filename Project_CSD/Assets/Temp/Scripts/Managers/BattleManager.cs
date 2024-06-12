using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Drawing;
using TMPro;
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
    [SerializeField] public Button reRoll;

    enum UnitType { Bread, Pupnut, Kitchu, Ramo, Sorang, Croirang }; // 테스트(제작)용
    UnitType unitType;

    private void Awake()
    {
        // 전투 시작
        battleState = BattleState.Start;
        wave = 0;
        Wave();

        // 전투 기본 세팅
        battle.SetActive(true);
        gameEnd.SetActive(false);
        CardMake();
        curHealth = maxHealth;
        UpdateHealthBar();

        spawnList = new List<Spawn>();

        ReadSpawnFile(wave); // 적 유닛 스폰 파일 가져오기

        unitType = UnitType.Bread; // 테스트(제작)용
    }

    void Update()
    {
        // 적 소환 -> 테스트 용
        if (Input.GetKeyDown("2")) { pool.Get(2, 0); }

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

                Wave();
            }
        }
        
        if(spawnIndex >= 1)
        {
            waveUI.SetActive(false);
        }


        // 플레이어 유닛 소환 -> 테스트(제작)용
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            unitType = UnitType.Bread;
        }
        else if (Input.GetKey(KeyCode.Keypad1))
        {
            unitType = UnitType.Pupnut;
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            unitType = UnitType.Kitchu;
        }
        else if (Input.GetKey(KeyCode.Keypad3))
        {
            unitType = UnitType.Ramo;
        }
        else if (Input.GetKey(KeyCode.Keypad4))
        {
            unitType = UnitType.Sorang;
        }
        else if (Input.GetKey(KeyCode.Keypad5))
        {
            unitType = UnitType.Croirang;
        }

        // 유닛 소환 영역 활성화
        if (Input.GetKeyDown("1"))
        {
            GameObject spawnArea = unitSpawnRange.transform.GetChild(1).gameObject;
            RectTransform spawnAreaAnchors = spawnArea.GetComponent<RectTransform>();

            // 메인 카메라의 위치에 따라 스폰 가능 영역 범위 변경
            if (mainCamera.position.x >= 3)
            {
                spawnAreaAnchors.anchorMin = new Vector2(0, 0.1f);
                spawnAreaAnchors.anchorMax = new Vector2(1, 0.5f);
            }
            else
            {
                spawnAreaAnchors.anchorMin = new Vector2(0.15f, 0.1f);
                spawnAreaAnchors.anchorMax = new Vector2(1, 0.5f);
            }
            unitSpawnRange.SetActive(true);
        }
    }

    void Wave()
    {
        // Wave UI 활성화
        waveUI.SetActive(true);

        int waveCount = wave + 1;

        // 숫자 이미지 변경
        int ten = waveCount / 10 > 0 ? waveCount / 10 : 0;
        int one = waveCount % 10;

        battleWaveImg[0].sprite = waveNumImg[one];
        battleWaveImg[1].sprite = waveNumImg[ten];

        // Wave 애니메이션
        foreach (Animator waveAnim in waveUIChild)
        {
            waveAnim.SetBool("next", true);
        }
    }

    public void EndGame(string whether)
    {
        resultUI.SetActive(true);

        int waveCount = wave + 1;

        // 숫자 이미지 변경
        int ten = waveCount / 10 > 0 ? waveCount / 10 : 0;
        int one = waveCount % 10;

        resultWaveImg[0].sprite = waveNumImg[one];
        resultWaveImg[1].sprite = waveNumImg[ten];

        if (whether == "Win")
        {
            resultPanel.sprite = resultImg[0];
            resultMenuBar.sprite = resultImg[2];

            StartCoroutine(ResultUI(2));
        }
        else if(whether == "Lose")
        {
            resultPanel.sprite = resultImg[1];
            resultMenuBar.sprite = resultImg[3];

            Debug.Log("Lose");
            StartCoroutine(ResultUI(0));
        }
    }

    IEnumerator ResultUI(int second)
    {
        Time.timeScale = 1;

        yield return new WaitForSeconds(second);

        // 애니메이션
        foreach (Animator anim in resultObjsAnim)
        {
            anim.SetBool("end", true);
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
        if (UiManager.Instance.cost >= 1&&Recost)
        {
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

        // 리스폰 파일 읽기
        TextAsset textFile = Resources.Load(enemySpawnFile[waveCount].name) as TextAsset; // as 를 사용해서 text 파일인지 검증 -> 아니면 null 처리됨.
        StringReader reader = new StringReader(textFile.text); // StringReader : 파일 내의 문자열 데이터 읽기 클래스 - > 파일 열기

        // 한 줄씩 데이터 저장
        while (reader != null)
        {
            string line = reader.ReadLine(); // ReadLine() : 자동 줄 바꿈 -> 텍스트 데이터를 한 줄씩 반환.

            if (line == null)
                break;

            Spawn spawnData = new Spawn();
            spawnData.spawnDelay = float.Parse(line.Split(',')[0]);
            spawnData.unitType = int.Parse(line.Split(',')[1]);
            spawnData.unitIndex = int.Parse(line.Split(',')[2]);
            spawnData.spawnPoint = int.Parse(line.Split(',')[3]);
            spawnList.Add(spawnData); // 리스트에 값을 저장
        }

        // 텍스트 파일 닫기
        reader.Close(); // 파일 닫기

        // 미리 첫번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].spawnDelay;
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
}
