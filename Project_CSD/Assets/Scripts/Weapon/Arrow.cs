using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    public GameObject target;
    public GameObject playerUnit;
    public float speed = 10f;
    public Vector3 movePosition;
    private float playerX;
    private float targetX;
    private float nextX;
    private float dist;
    private float baseY;
    private float height;

    public int unitType;
    public float arrowPower;

    void Update()
    {
        // 오류 방지
        if (target == null)
            return;

        // 포물선 공식
        playerX = transform.position.x;
        targetX = target.transform.position.x;
        dist = targetX - playerX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(transform.position.y, target.transform.position.y, (nextX - playerX) / dist);
        height =  0.8f * (nextX - playerX) * (nextX - targetX) / ((targetX > playerX ? -0.25f : 0.25f) * dist) * 0.1f;
        movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        // 쏘는 방향에 따라 Sprite 방향 
        transform.localScale = new Vector3(-1f, 1f, 1f);

        if (movePosition == target.transform.position)
        {
            if (target.CompareTag("Wall"))
            {
                MainWall wallLogic = target.GetComponent<MainWall>();
                wallLogic.health -= arrowPower;
            } else
            {
                if(unitType == 1)
                {
                    // 아군 유닛 기준 로직
                    EnemyUnit enemy = target.GetComponent<EnemyUnit>();

                    enemy.health -= arrowPower;
                    enemy.unitActivity = UnitBase.UnitActivity.Normal;
                } else
                {
                    // 적 유닛 기준 로직
                    PlayerUnit enemy = target.GetComponent<PlayerUnit>();

                    enemy.health -= arrowPower;
                    enemy.unitActivity = UnitBase.UnitActivity.Normal;
                }
            }

            // 화살 비활성화
            gameObject.SetActive(false);
        }
    }

    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }
}
