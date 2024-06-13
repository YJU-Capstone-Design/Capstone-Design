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

    [Header("HpBar")] // ���� ��
    public float curHealth; //* ���� ü��
    public float maxHealth; //* �ִ� ü��
    public GameObject healthBar; // �� ü�¹�
    public Slider HpBarSlider;

    [Header("BattleMgr")]
    [SerializeField] private GameObject battle;
    [SerializeField] private GameObject gameEnd;
    [SerializeField] public Transform mainCamera;
    [SerializeField] public int wave;

    [Header("Spawn")]
    public PoolManager pool;
    public Vector3 point; // ���콺 Ŭ�� ����Ʈ
    public Transform[] unitSpawnPoint; // �⺻ spawn point
    [SerializeField] List<TextAsset> enemySpawnFile; // Enemy Spawn �� �����ִ� Text File
    [SerializeField] List<Spawn> spawnList; // Text File ���� �о���� ���� �����Ű�� ���� List
    int spawnIndex; // ���� ���� ����
    bool spawnEnd; // ���� ���� ������
    float curSpawnTime;
    float nextSpawnDelay;
    public GameObject unitSpawnRange; // ���� ���� ���� (canvas ������Ʈ)

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

    enum UnitType { Bread, Pupnut, Kitchu, Ramo, Sorang, Croirang }; // �׽�Ʈ(����)��
    UnitType unitType;

    private void Awake()
    {
        // ���� ����
        battleState = BattleState.Start;
        wave = 0;
        Wave();

        // ���� �⺻ ����
        battle.SetActive(true);
        gameEnd.SetActive(false);
        CardMake();
        curHealth = maxHealth;
        UpdateHealthBar();

        spawnList = new List<Spawn>();

        ReadSpawnFile(wave); // �� ���� ���� ���� ��������

        unitType = UnitType.Bread; // �׽�Ʈ(����)��
    }

    void Update()
    {
        // �� ��ȯ -> �׽�Ʈ ��
        if (Input.GetKeyDown("2")) { pool.Get(2, 0); }

        if (CardManager.Instance.enemys.Count > 0 && curHealth > 0)
        {
            battleState = BattleState.Start;
        }

        if (battleState != BattleState.Start)
            return;

        if (!spawnEnd) { curSpawnTime += Time.deltaTime; } // ������ ������ ��� ���� Ÿ���� ���� X

        // ���� ����
        if (curSpawnTime > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnTime = 0;
        }

        // ��� ���� ��ȯ�� ��, �ʵ忡 �����ִ� ���� ����, ���� hp�� ���� ���� �� ��Ȳ ó��
        if (spawnEnd && CardManager.Instance.enemys.Count == 0 && curHealth > 0)
        {
            Debug.Log("End Wave");
            // ��� enemySpawnFile �� �� ó������ ��� Win
            if (wave + 1 == enemySpawnFile.Count)
            {
                battleState = BattleState.Win;
                EndGame("Win");
            }
            // ���� ó������ ���� enemySpawnFile �� �������� ��� ���� Wave ����
            else if (wave + 1 < enemySpawnFile.Count && battleState == BattleState.Start)
            {
                Debug.Log("Next Wave");

                battleState = BattleState.BreakTime;
                wave++;
                ReadSpawnFile(wave); // �� ���� ���� ���� ��������
                battleState = BattleState.Start;

                Wave();
            }
        }
        
        if(spawnIndex >= 1)
        {
            waveUI.SetActive(false);
        }


        // �÷��̾� ���� ��ȯ -> �׽�Ʈ(����)��
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

        // ���� ��ȯ ���� Ȱ��ȭ
        if (Input.GetKeyDown("1"))
        {
            GameObject spawnArea = unitSpawnRange.transform.GetChild(1).gameObject;
            RectTransform spawnAreaAnchors = spawnArea.GetComponent<RectTransform>();

            // ���� ī�޶��� ��ġ�� ���� ���� ���� ���� ���� ����
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
        // Wave UI Ȱ��ȭ
        waveUI.SetActive(true);

        int waveCount = wave + 1;

        // ���� �̹��� ����
        int ten = waveCount / 10 > 0 ? waveCount / 10 : 0;
        int one = waveCount % 10;

        battleWaveImg[0].sprite = waveNumImg[one];
        battleWaveImg[1].sprite = waveNumImg[ten];

        // Wave �ִϸ��̼�
        foreach (Animator waveAnim in waveUIChild)
        {
            waveAnim.SetBool("next", true);
        }
    }

    public void EndGame(string whether)
    {
        resultUI.SetActive(true);

        int waveCount = wave + 1;

        // ���� �̹��� ����
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

        // �ִϸ��̼�
        foreach (Animator anim in resultObjsAnim)
        {
            anim.SetBool("end", true);
        }
    }

    // ���� ���� ��ư
    public void UnitSpawn()
    {
        // ���콺 ��Ŭ�� �� ���� ��ġ��
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
                EndGame("Lose"); // ���â UI Ȱ��ȭ
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
        // ���� �ʱ�ȭ
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // ������ ���� �б�
        TextAsset textFile = Resources.Load(enemySpawnFile[waveCount].name) as TextAsset; // as �� ����ؼ� text �������� ���� -> �ƴϸ� null ó����.
        StringReader reader = new StringReader(textFile.text); // StringReader : ���� ���� ���ڿ� ������ �б� Ŭ���� - > ���� ����

        // �� �پ� ������ ����
        while (reader != null)
        {
            string line = reader.ReadLine(); // ReadLine() : �ڵ� �� �ٲ� -> �ؽ�Ʈ �����͸� �� �پ� ��ȯ.

            if (line == null)
                break;

            Spawn spawnData = new Spawn();
            spawnData.spawnDelay = float.Parse(line.Split(',')[0]);
            spawnData.unitType = int.Parse(line.Split(',')[1]);
            spawnData.unitIndex = int.Parse(line.Split(',')[2]);
            spawnData.spawnPoint = int.Parse(line.Split(',')[3]);
            spawnList.Add(spawnData); // ����Ʈ�� ���� ����
        }

        // �ؽ�Ʈ ���� �ݱ�
        reader.Close(); // ���� �ݱ�

        // �̸� ù��° ���� ������ ����
        nextSpawnDelay = spawnList[0].spawnDelay;
    }

    void SpawnEnemy()
    {
        Spawn list = spawnList[spawnIndex];

        PoolManager.Instance.Get(list.unitType, list.unitIndex, list.spawnPoint);

        // ������ �ε��� ����
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            curSpawnTime = 0;
            spawnIndex = 0;
            return;
        }

        // ���� ������ ������ ����
        nextSpawnDelay = spawnList[spawnIndex].spawnDelay;
    }
}
