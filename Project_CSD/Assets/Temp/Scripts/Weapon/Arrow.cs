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

    public int unitType; // ȭ���� ��� Unit �� ����
    public float arrowPower; // ȭ���� �ִ� ������

    void Update()
    {
        // ���� ����
        if (target == null)
        {
            // ������Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);
            return;
        }
        else if (!target.activeInHierarchy)
        {
            // ������Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        // ������ ����
        playerX = transform.position.x;
        targetX = target.transform.position.x;

        dist = targetX - playerX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(transform.position.y, target.transform.position.y, (nextX - playerX) / dist);
        height =  0.8f * (nextX - playerX) * (nextX - targetX) / ((targetX > playerX ? -0.25f : 0.25f) * dist) * 0.1f;
        movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        // ��ǥ ���� ����
        if (movePosition == target.transform.position)
        {
            Arrived();

            // ������Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }

    // Rotation ����
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
                // �Ʊ� ���� ���� ����
                EnemyUnit enemy = target.GetComponent<EnemyUnit>();

                enemy.health -= arrowPower;
            }
            else
            {
                // �� ���� ���� ����
                PlayerUnit enemy = target.GetComponent<PlayerUnit>();

                enemy.health -= arrowPower;
            }
        }
    }
}
