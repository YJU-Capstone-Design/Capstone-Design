using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Drawing;
public class BattleManager :Singleton<BattleManager>
{
    public enum BattleState { Start, Win, Lose }
    public BattleState battleState;

    [Header("Shop")]
    [SerializeField] private Transform shopParent;
    [SerializeField] private GameObject card;
    public List<GameObject> cardObj = new List<GameObject>();

    [Header("HpBar")] // 메인 집
    public float curHealth; //* 현재 체력
    public float maxHealth; //* 최대 체력
    public GameObject healthBar; // 벽 체력바
    public Slider HpBarSlider; 

    [Header("BattleMgr")]
    [SerializeField] private GameObject battle;
    [SerializeField] private GameObject gameEnd;

    [Header("Spawn")]
    public PoolManager pool;
    public Vector3 point; // 마우스 클릭 포인트
    public Transform[] unitSpawnPoint; // 기본 spawn point
    [SerializeField] List<Spawn> spawnList;
    int spawnIndex; // 스폰 로직 순서
    bool spawnEnd; // 스폰 로직 마지막
    float curSpawnTime;  
    float nextSpawnDelay;
    public GameObject hpBarParent; // 유닛 체력바 부모 (canvas 오브젝트)
    public GameObject unitSpawnRange; // 유닛 스폰 범위 (canvas 오브젝트)

    enum UnitType {Bread, Pupnut, Kitchu, Ramo}; // 테스트(제작)용
    UnitType unitType;

    private void Awake()
    {
        // 전투 시작
        battleState = BattleState.Start;

        // 전투 기본 세팅
        battle.SetActive(true);
        gameEnd.SetActive(false);
        CardMake();
        curHealth = maxHealth;
        UpdateHealthBar();

        spawnList = new List<Spawn>();

        ReadSpawnFile(); // 적 유닛 스폰 파일 가져오기

        unitType = UnitType.Bread; // 테스트(제작)용
    }

    void Update()
    {
        if (battleState != BattleState.Start)
            return;

        if(!spawnEnd) { curSpawnTime += Time.deltaTime; }

        // 모든 적이 소환된 후, 필드에 남아있는 적이 없고, 벽의 hp가 남아 있으면 Win
        if(spawnEnd && CardManger.Instance.enemys.Count == 0 && curHealth > 0)
        {
            battleState = BattleState.Win;
        }

        // 몬스터 스폰
        if (curSpawnTime > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnTime = 0;
        }

        // 플레이어 유닛 소환 -> 테스트(제작)용
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            unitType = UnitType.Bread;
        } 
        else if(Input.GetKey(KeyCode.Keypad1))
        {
            unitType= UnitType.Pupnut;
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            unitType = UnitType.Kitchu;
        }
        else if (Input.GetKey(KeyCode.Keypad3))
        {
            unitType = UnitType.Ramo;
        }

        // 유닛 소환 영역 활성화
        if (Input.GetKeyDown("1"))
        {
            unitSpawnRange.SetActive(true);
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
            GameObject myInstance = Instantiate(card, shopParent);
            cardObj.Add(myInstance);
        }


    }

    public void CardShuffle()
    {
       
        foreach(GameObject card in cardObj)
        {
            Destroy(card);
        }
        cardObj.Clear();
        CardMake();
        Debug.Log("Shuffle");
    }

    void UpdateHealthBar()
    {
        
        float sliderValue = curHealth / maxHealth;
        HpBarSlider.value = sliderValue;
        if (curHealth <= 0)
        {
            healthBar.SetActive(false);
            battle.SetActive(false);
            gameEnd.SetActive(true);
        }
    }

    void ReadSpawnFile()
    {
        // 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // 리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage1") as TextAsset; // as 를 사용해서 text 파일인지 검증 -> 아니면 null 처리됨.
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
            return;
        }

        // 다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].spawnDelay;
    }
}
