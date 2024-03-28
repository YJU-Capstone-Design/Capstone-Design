using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    // 프리팹
    public GameObject[][] prefabs;
    public GameObject[] unitPrefabs;
    public GameObject[] spellPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] weaponPrefabs;

    // 풀 담당을 하는 리스트들
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

    public GameObject Get(int prefabIndex,int objIndex)
    {
        int ran;
        GameObject select = null;

        // 선택한 풀이 놀고 (비활성화 된) 있는 게임 오브젝트 접근
        foreach (GameObject item in pools[prefabIndex])
        {
            if (!item.activeSelf && item == prefabs[prefabIndex][objIndex])
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 모든 오브젝트 사용 중일 시 (못 찾았을 때) 오브젝트를 새로 생성해서 select 변수에 할당
        if (select == null)
        {
            select = Instantiate(prefabs[prefabIndex][objIndex], transform);
            // transform -> 오브젝트 생성위치 (= 부모 오브젝트 -> 자기자신(PoolManager))

            // 생성된 오브젝트는 해당 오브젝트 풀 리스트에 추가
            pools[prefabIndex].Add(select);
        }

        // 스폰 포인트
        switch(prefabIndex)
        {
            case 0:
            case 1:
                select.transform.position = GameManager.Instance.unitSpawnPoint[0].position;
                break;
            case 2:
                ran = UnityEngine.Random.Range(1, 4);
                select.transform.position = GameManager.Instance.unitSpawnPoint[ran].position;
                select.SetActive(true);
                break;
        }

        return select;
    }
    
    // 포지션 값이 다른 아이템 때문에 추가    ex) 화살
    public GameObject Get(int prefabIndex, int objIndex, Vector3 startPos)
    {
        GameObject select = Get(prefabIndex, objIndex);

        // 스폰 포인트
        select.transform.position = startPos;

        return select;
    }
}
