using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Drawing;
public class BattleManager :Singleton<BattleManager>
{
    [Header("Shop")]
    [SerializeField] private Transform shopParent;
    [SerializeField] private GameObject card;

    [Header("HpVar")]
    public float curHealth; //* 현재 체력
    public float maxHealth; //* 최대 체력
    public GameObject healthBar; //
    public Slider HpBarSlider;

    [Header("BattleMgr")]
    [SerializeField] private GameObject battle;
    [SerializeField] private GameObject gameEnd;

    [Header("Spawn")]
    public PoolManager pool;
    public Vector3 point;
    public Transform[] unitSpawnPoint; // 기본 spawn point
    [SerializeField] List<Spawn> spawnList;
    int spawnIndex; // 스폰 로직 순서
    bool spawnEnd; // 스폰 로직 마지막
    float curSpawnTime;
    float nextSpawnDelay;

    private void Awake()
    {
        battle.SetActive(true);
        gameEnd.SetActive(false);
        CardMake();
        curHealth = maxHealth;
        UpdateHealthBar();

        spawnList = new List<Spawn>();

        ReadSpawnFile();
    }

    void Update()
    {
        //curSpawnTime += Time.deltaTime;

        if (curSpawnTime > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnTime = 0;
        }

        // 플레이어 근접/궁수 유닛 소환
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // 마우스 좌클릭 한 곳의 위치값
            point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));

            // 클릭한 곳 범위 조정
            point = MoveRange(point);

            if (Input.GetMouseButtonDown(0)) { pool.Get(0, 0); }
            else if (Input.GetMouseButtonDown(1)) { pool.Get(0, 1); }
        }


        // 적 좀비(근접) 유닛 소환
        if (Input.GetKeyDown("1"))
        {
            pool.Get(2, 0);
        }

        // 적 좀비(궁수) 유닛 소환
        if (Input.GetKeyDown("2"))
        {
            pool.Get(2, 1);
        }

        // 적 키클롭스(탱커) 유닛 소환
        if (Input.GetKeyDown("3"))
        {
            pool.Get(2, 2);
        }

        // 적 스켈레톤(근접) 유닛 소환
        if (Input.GetKeyDown("4"))
        {
            pool.Get(2, 3);
        }
    }

    // 경계선 범위 벗어나지 않게 이동 범위 조정
    Vector3 MoveRange(Vector3 vec)
    {
        // 마우스로 클릭한 곳 범위 조정
        // 경계선 범위 벗어나지 않게 설정
        if (vec.y >= 2) { vec.y = 2; }
        else if (vec.y <= -2) { vec.y = -2; }
        if (vec.x >= 6) { vec.x = 6; }
        else if (vec.x <= -6) { vec.x = -6; }

        return vec;
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
        }


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
