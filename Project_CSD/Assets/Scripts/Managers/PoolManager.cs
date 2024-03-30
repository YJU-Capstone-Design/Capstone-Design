using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    // ������
    public GameObject[][] prefabs;
    public GameObject[] unitPrefabs;
    public GameObject[] spellPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] weaponPrefabs;

    // Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] pools;

    void Awake()
    {
        prefabs = new GameObject[4][];
        pools = new List<GameObject>[prefabs.Length];

        prefabs[0] = unitPrefabs;
        prefabs[1] = spellPrefabs;
        prefabs[2] = enemyPrefabs;
        prefabs[3] = weaponPrefabs;

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    // �⺻ Pool
    public GameObject Get(int prefabIndex,int objIndex)
    {
        GameObject select = null;

        // ������ Ǯ�� ��� (��Ȱ��ȭ ��) �ִ� ���� ������Ʈ ����
        foreach (GameObject item in pools[prefabIndex])
        {
            if (!item.activeSelf && item == prefabs[prefabIndex][objIndex])
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ��� ������Ʈ ��� ���� �� (�� ã���� ��) ������Ʈ�� ���� �����ؼ� select ������ �Ҵ�
        if (select == null)
        {
            select = Instantiate(prefabs[prefabIndex][objIndex], transform);
            // transform -> ������Ʈ ������ġ (= �θ� ������Ʈ -> �ڱ��ڽ�(PoolManager))

            // ������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� �߰�
            pools[prefabIndex].Add(select);
        }

        // ���� ����Ʈ
        switch(prefabIndex)
        {
            case 0:
            case 1:
                select.transform.position = BattleManager.Instance.unitSpawnPoint[0].position;
                break;
            case 2:
                int ran = UnityEngine.Random.Range(1, 4);
                select.transform.position = BattleManager.Instance.unitSpawnPoint[ran].position;
                break;
        }

        return select;
    }
    
    // ���� ������ ���� �ٸ� ������ Pool    ex) ȭ��
    public GameObject Get(int prefabIndex, int objIndex, Vector3 startPos)
    {
        GameObject select = Get(prefabIndex, objIndex);

        // ���� ����Ʈ
        select.transform.position = startPos;

        return select;
    }

    // Enemy Unit ���� Pool
    public GameObject Get(int prefabIndex, int objIndex, int spawnPoint)
    {
        GameObject select = Get(prefabIndex, objIndex);

        // ���� ����Ʈ
        select.transform.position = BattleManager.Instance.unitSpawnPoint[spawnPoint+1].position;

        return select;
    }
}
