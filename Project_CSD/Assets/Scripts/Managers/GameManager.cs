using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PoolManager pool;

    public Transform[] unitSpawnPoint;

    public Vector3 point;

    
    void Update()
    {
        // 플레이어 근접 유닛 소환
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 좌클릭 한 곳의 위치값
            point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));

            pool.Get(0, 0);
        }

        // 플레이어 궁수 유닛 소환
        if (Input.GetMouseButtonDown(1))
        {
            // 마우스 좌클릭 한 곳의 위치값
            point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));

            pool.Get(0, 1);
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
}
