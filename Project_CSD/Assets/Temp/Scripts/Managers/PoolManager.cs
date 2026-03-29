using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject[][] prefabs;
    public GameObject[] unitPrefabs;
    public GameObject[] spellPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] weaponPrefabs;

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

    // ─────────────────────────────────────────────
    // 기본 Pool (unitSpawnPoint[0] 고정 스폰)
    // ─────────────────────────────────────────────
    public GameObject Get(int prefabIndex, int objIndex)
    {
        GameObject select = null;

        foreach (GameObject item in pools[prefabIndex])
        {
            if (!item.activeInHierarchy &&
                item.name == prefabs[prefabIndex][objIndex].name + "(Clone)")
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(prefabs[prefabIndex][objIndex], transform);
            pools[prefabIndex].Add(select);
        }

        switch (prefabIndex)
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

    // ─────────────────────────────────────────────
    // 시작 포지션이 다른 아이템 Pool (ex: 화살)
    // ─────────────────────────────────────────────
    public GameObject Get(int prefabIndex, int objIndex, Vector3 startPos)
    {
        GameObject select = Get(prefabIndex, objIndex);
        select.transform.position = startPos;
        return select;
    }

    // ─────────────────────────────────────────────
    // Enemy Unit 생성 Pool
    // ─────────────────────────────────────────────
    public GameObject Get(int prefabIndex, int objIndex, int spawnPoint)
    {
        GameObject select = Get(prefabIndex, objIndex);
        select.transform.position = BattleManager.Instance.unitSpawnPoint[spawnPoint + 1].position;
        return select;
    }

    // ─────────────────────────────────────────────
    // 드래그 소환 전용: BattleManager.point 위치에 스폰
    // ─────────────────────────────────────────────
    public GameObject GetAtPoint(int prefabIndex, int objIndex)
    {
        GameObject select = null;

        foreach (GameObject item in pools[prefabIndex])
        {
            if (!item.activeInHierarchy &&
                item.name == prefabs[prefabIndex][objIndex].name + "(Clone)")
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(prefabs[prefabIndex][objIndex], transform);
            pools[prefabIndex].Add(select);
        }

        Vector3 spawnPos = BattleManager.Instance.point;
        spawnPos.z = 0f;
        select.transform.position = spawnPos;

        return select;
    }
}