using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    public GameObject target;
    public float speed = 10f;
    public Vector3 movePosition;
    private float playerX;
    private float targetX;
    private float nextX;
    private float dist;
    private float baseY;
    private float height;


    void Update()
    {
        playerX = transform.position.x;
        targetX = target.transform.position.x;
        dist = targetX - playerX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(transform.position.y, target.transform.position.y, (nextX - playerX) / dist);
        height =  0.8f * (nextX - playerX) * (nextX - targetX) / ((targetX > playerX ? -0.25f : 0.25f) * dist) * 0.1f;
        movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        // 쏘는 방향에 따라 Sprite 방향 변경
        if (targetX > playerX)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (targetX < playerX)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (movePosition == target.transform.position)
        {
            gameObject.SetActive(false);
        }
    }
    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }
}
