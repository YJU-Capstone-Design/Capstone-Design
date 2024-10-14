using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Security.Cryptography;

public class Arrow : MonoBehaviour
{
    public GameObject target;
    public GameObject playerUnit;
    public float speed;
    public Vector3 movePosition;
    private float playerX;
    private float targetX;
    private float nextX;
    private float dist;
    private float baseY;
    private float height;

    public int unitType; // 화살을 쏘는 Unit 의 유형
    public float arrowPower; // 화살이 주는 데미지

    void Update()
    {
        // 오류 방지
        if (target == null)
        {
            // 오브젝트 비활성화
            gameObject.SetActive(false);
            return;
        }
        else if (!target.activeInHierarchy)
        {
            // 오브젝트 비활성화
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

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

        // 목표 지점 도달
        if (movePosition == target.transform.position)
        {
            Arrived();

            // 오브젝트 비활성화
            gameObject.SetActive(false);
        }
    }

    // Rotation 조정
    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }

    void Arrived()
    {
        if (target.CompareTag("Wall"))
        {
            BattleManager.Instance.curHealth -= arrowPower;
        }
        else
        {
            if (target == null)
                return;

            if (unitType == 1)
            {
                // 아군 유닛 기준 로직
                EnemyUnit enemy = target.GetComponent<EnemyUnit>();

                enemy.health -= arrowPower;
            }
            else
            {
                // 적 유닛 기준 로직
                PlayerUnit enemy = target.GetComponent<PlayerUnit>();

                enemy.health -= arrowPower;
            }
        }
    }
}
