using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEngine.GraphicsBuffer;

public class GameManager : Singleton<GameManager>
{
    public PoolManager pool;

    public Transform[] unitSpawnPoint;

    public Vector3 point;

    
    void Update()
    {
        // �÷��̾� ����/�ü� ���� ��ȯ
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // ���콺 ��Ŭ�� �� ���� ��ġ��
            point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));

            // Ŭ���� �� ���� ����
            point = MoveRange(point);

            if (Input.GetMouseButtonDown(0)) { pool.Get(0, 0); }
            else if(Input.GetMouseButtonDown(1)) { pool.Get(0, 1); }
        }


        // �� ���� ���� ��ȯ
        if (Input.GetKeyDown("1"))
        {
            pool.Get(2, 0);
        }

        // �� �ü� ���� ��ȯ
        if(Input.GetKeyDown("2"))
        {
            pool.Get(2, 1);
        }
    }

    // ��輱 ���� ����� �ʰ� �̵� ���� ����
    Vector3 MoveRange(Vector3 vec)
    {
        // ���콺�� Ŭ���� �� ���� ����
        // ��輱 ���� ����� �ʰ� ����
        if (vec.y >= 2) { vec.y = 2; }
        else if (vec.y <= -2) { vec.y = -2; }
        else if (vec.x >= 6) { vec.x = 6; }
        else if (vec.x <= -7) { vec.x = -7; }

        return vec;
    }
}
