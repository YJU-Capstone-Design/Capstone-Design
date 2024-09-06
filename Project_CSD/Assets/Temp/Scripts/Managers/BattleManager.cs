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
    [SerializeField] Button[] resultButtons;
    [SerializeField] TextMeshProUGUI rainkg_Btn_Text;
    [SerializeField] public Button reRoll;

    enum UnitType { Bread, Pupnut, Kitchu, Ramo, Sorang, Croirang }; // �׽�Ʈ(����)��
    UnitType unitType;

    [Header("# Ÿ�̸ӿ� ���̺�")]
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI result_Time;
    public float timer = 180;
    private float limite_time = 0f;
    [SerializeField] Image waveImg;
    [SerializeField] Image waveImg2;
    public float endTime=0f;
    bool victory = false;

    [Header("# ��ŷ")]
    [SerializeField] GameObject rank_Obj;
    [SerializeField] GameObject rank_Item;
    [SerializeField] TMP_InputField player_Name;
    [SerializeField] TMP_InputField player_No;

    private void Awake()
    {
        // ���� ����
        battleState = BattleState.Start;
        wave = 0;
        StartCoroutine(Wave());

        // ���� �⺻ ����
        battle.SetActive(true);
        gameEnd.SetActive(false);
        CardMake();
        if (PlayerData.instance != null) { if (PlayerData.instance.mainHp_Stu >= 1) { maxHealth *= PlayerData.instance.mainHp_Stu; } }
        curHealth = maxHealth;
        UpdateHealthBar();

        spawnList = new List<Spawn>();

        ReadSpawnFile(wave); // �� ���� ���� ���� ��������

        unitType = UnitType.Bread; // �׽�Ʈ(����)��
   
    }

    void Update()
    {
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
                unitSpawnRange.SetActive(false);
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

                StartCoroutine(Wave());
            }
        }

        //Invoke("BattleTimer", 2f);
        BattleTimer();
    }

    void BattleTimer()//Ÿ�̸�
    {
        //Ŭ��������� �ð�

        
        limite_time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(limite_time / 60);
        int seconds = Mathf.FloorToInt(limite_time % 60);

        time.text = string.Format("{0:00} : {1:00}", minutes, seconds);

        //Ÿ�� ����
        /*limite_time -= Time.deltaTime;
        if (limite_time <= 0)
        {
            battleState = BattleState.Win;
            unitSpawnRange.SetActive(false);
            EndGame("Win");
        }
        if (limite_time >= 0)
        {
            int minutes = Mathf.FloorToInt(limite_time / 60);
            int seconds = Mathf.FloorToInt(limite_time % 60);

            time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        }*/


    }

    IEnumerator Wave()
    {
        // Wave UI Ȱ��ȭ
        waveUI.SetActive(true);

        int waveCount = wave + 1;

        // ���� �̹��� ����
        int ten = waveCount / 10 > 0 ? waveCount / 10 : 0;
        int one = waveCount % 10;

        battleWaveImg[0].sprite = waveNumImg[one];
        battleWaveImg[1].sprite = waveNumImg[ten];
        waveImg.sprite = waveNumImg[ten];
        waveImg2.sprite = waveNumImg[one];
        // Wave �ִϸ��̼�
        foreach (Animator waveAnim in waveUIChild)
        {
            waveAnim.SetBool("next", true);
        }

        yield return new WaitForSeconds(2.2f);

        // Wave UI ��Ȱ��ȭ
        waveUI.SetActive(false);
    }

    public void EndGame(string whether)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound();}

        resultUI.SetActive(true);

        int waveCount = wave + 1;

        // ���� �̹��� ����
        int ten = waveCount / 10 > 0 ? waveCount / 10 : 0;
        int one = waveCount % 10;

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
            endTime = limite_time;
            int minutes = Mathf.FloorToInt(endTime / 60);
            int seconds = Mathf.FloorToInt(endTime % 60);
            result_Time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
            victory = true;

        }
        else if(whether == "Lose")
        {
            resultPanel.sprite = resultImg[1];
            resultMenuBar.sprite = resultImg[3];
            if (AudioManager.instance != null) { AudioManager.instance.BattleEndSound(false); }
            Debug.Log("Lose");
            StartCoroutine(ResultUI(0));
            endTime = limite_time;
            int minutes = Mathf.FloorToInt(endTime / 60);
            int seconds = Mathf.FloorToInt(endTime % 60);
            result_Time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        }
    }

    // ���� �¸� ��, �����ͺ��̽��� ������ �Է� ��ư �Լ�
    public void SaveUserData()
    {
        if(!string.IsNullOrEmpty(player_Name.text) && !string.IsNullOrEmpty(player_No.text))
        {
            Debug.Log(player_Name.text + "    " + player_Name.text);
            // �����ͺ��̽� �Է�
            XmlNodeList selectedData = DBConnect.Select("ranking", $"WHERE id = {int.Parse(player_No.text)}");

            // player_No ���� int ������ Ȯ�� ��, ������ �Է�
            if (int.TryParse(player_No.text, out int playerId))
            {
                if (selectedData == null)
                {
                    DBConnect.Insert("ranking", int.Parse(player_No.text), player_Name.text, (int)endTime);
                }
                else
                {
                    DBConnect.Update("ranking", "name", "time", player_Name.text, (int)endTime, $"id = {int.Parse(player_No.text)}");
                }

                // �κ� ���� ���� �߰� �ʿ�
            }
            else
            {
                Debug.Log("�й��� �߸��Ǿ����ϴ�.");
            }
        }
        else
        {
            Debug.Log("�Է°��� ����ֽ��ϴ�.");
        }
    }

    IEnumerator ResultUI(int second)
    {
        Time.timeScale = 1;

        yield return new WaitForSeconds(second);

        // �ִϸ��̼�
        for (int i = 0; i < resultObjsAnim.Length; i++)
        {
            Animator anim = resultObjsAnim[i];
            if (i == resultObjsAnim.Length - 1 && !victory || i == resultObjsAnim.Length - 2 && !victory)
            {
                // victory�� false�� ���, ������ ������Ʈ�� �ִϸ��̼��� �������� ����
                continue;
            }
            anim.SetBool("end", true);
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < resultButtons.Length; i++)
        {
            Button resultBtn = resultButtons[i];
            if (i == resultButtons.Length - 1 && !victory)
            {
                // victory�� false�� ���, ������ ��ư�� Ȱ��ȭ���� ����
                continue;
            }
            resultBtn.enabled = true;
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
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
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
