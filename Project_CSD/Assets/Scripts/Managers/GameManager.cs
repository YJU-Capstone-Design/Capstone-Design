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
        // 플레이어 근접/궁수 유닛 소환
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // 마우스 좌클릭 한 곳의 위치값
            point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));

            // 클릭한 곳 범위 조정
            point = MoveRange(point);

            if (Input.GetMouseButtonDown(0)) { pool.Get(0, 0); }
            else if(Input.GetMouseButtonDown(1)) { pool.Get(0, 1); }
        }


        // 적 근접 유닛 소환
        if (Input.GetKeyDown("1"))
        {
            pool.Get(2, 0);
        }

        // 적 궁수 유닛 소환
        if(Input.GetKeyDown("2"))
        {
            pool.Get(2, 1);
        }
    }

    // 경계선 범위 벗어나지 않게 이동 범위 조정
    Vector3 MoveRange(Vector3 vec)
    {
        // 마우스로 클릭한 곳 범위 조정
        // 경계선 범위 벗어나지 않게 설정
        if (vec.y >= 2) { vec.y = 2; }
        else if (vec.y <= -2) { vec.y = -2; }
        else if (vec.x >= 6) { vec.x = 6; }
        else if (vec.x <= -7) { vec.x = -7; }

        return vec;
    }
}
