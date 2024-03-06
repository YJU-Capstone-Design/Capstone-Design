using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PoolManager pool;

    public Transform[] unitSpawnPoint;

    public Vector3 point;

    void Awake()
    {
        instance = this;
    }

    
    void Update()
    {
        // �÷��̾� ���� ��ȯ
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 ��Ŭ�� �� ���� ��ġ��
            point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));

            pool.Get(0);
        }

        // �� ���� ��ȯ
        if (Input.GetMouseButtonDown(1))
        {
            pool.Get(1);
        }
    }
}
