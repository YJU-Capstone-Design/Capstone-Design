using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    // ������
    public GameObject[] prefabs;

    // Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        int ran;
        GameObject select = null;

        // ������ Ǯ�� ��� (��Ȱ��ȭ ��) �ִ� ���� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
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
            select = Instantiate(prefabs[index], transform);
            // transform -> ������Ʈ ������ġ (= �θ� ������Ʈ -> �ڱ��ڽ�(PoolManager))

            // ������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� �߰�
            pools[index].Add(select);
        }

        // ���� ����Ʈ
        switch(index)
        {
            case 0:
            case 1:
                select.transform.position = GameManager.Instance.unitSpawnPoint[0].position;
                break;
            case 2:
                ran = Random.Range(1, 4);
                select.transform.position = GameManager.Instance.unitSpawnPoint[ran].position;
                break;
        }

        return select;
    }

    public GameObject Get(int index, Vector3 startPos)
    {
        GameObject select = null;

        // ������ Ǯ�� ��� (��Ȱ��ȭ ��) �ִ� ���� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
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
            select = Instantiate(prefabs[index], transform);
            // transform -> ������Ʈ ������ġ (= �θ� ������Ʈ -> �ڱ��ڽ�(PoolManager))

            // ������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� �߰�
            pools[index].Add(select);
        }

        // ���� ����Ʈ
        switch (index)
        {
            case 3:
                select.transform.position = startPos;
                break;
        }

        return select;
    }
}
