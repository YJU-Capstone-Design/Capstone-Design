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
    public float curHealth; //* ���� ü��
    public float maxHealth; //* �ִ� ü��
    public GameObject healthBar; //
    public Slider HpBarSlider;

    [Header("BattleMgr")]
    [SerializeField] private GameObject battle;
    [SerializeField] private GameObject gameEnd;

    [Header("Spawn")]
    public PoolManager pool;
    public Vector3 point;
    public Transform[] unitSpawnPoint; // �⺻ spawn point
    [SerializeField] List<Spawn> spawnList;
    int spawnIndex; // ���� ���� ����
    bool spawnEnd; // ���� ���� ������
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

        // �÷��̾� ����/�ü� ���� ��ȯ
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // ���콺 ��Ŭ�� �� ���� ��ġ��
            point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));

            // Ŭ���� �� ���� ����
            point = MoveRange(point);

            if (Input.GetMouseButtonDown(0)) { pool.Get(0, 0); }
            else if (Input.GetMouseButtonDown(1)) { pool.Get(0, 1); }
        }


        // �� ����(����) ���� ��ȯ
        if (Input.GetKeyDown("1"))
        {
            pool.Get(2, 0);
        }

        // �� ����(�ü�) ���� ��ȯ
        if (Input.GetKeyDown("2"))
        {
            pool.Get(2, 1);
        }

        // �� ŰŬ�ӽ�(��Ŀ) ���� ��ȯ
        if (Input.GetKeyDown("3"))
        {
            pool.Get(2, 2);
        }

        // �� ���̷���(����) ���� ��ȯ
        if (Input.GetKeyDown("4"))
        {
            pool.Get(2, 3);
        }
    }

    // ��輱 ���� ����� �ʰ� �̵� ���� ����
    Vector3 MoveRange(Vector3 vec)
    {
        // ���콺�� Ŭ���� �� ���� ����
        // ��輱 ���� ����� �ʰ� ����
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
        // ���� �ʱ�ȭ
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // ������ ���� �б�
        TextAsset textFile = Resources.Load("Stage1") as TextAsset; // as �� ����ؼ� text �������� ���� -> �ƴϸ� null ó����.
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
            return;
        }

        // ���� ������ ������ ����
        nextSpawnDelay = spawnList[spawnIndex].spawnDelay;
    }
}
