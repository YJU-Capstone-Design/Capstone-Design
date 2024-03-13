using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitBase;

public class MainWall : MonoBehaviour
{
    public float health;
    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if(health <= 0)
        {
            StartCoroutine(Break());
        }
    }

    IEnumerator Break()
    {
        // �� �ڸ����� �ִϸ��̼� �ʿ�

        yield return new WaitForSeconds(1f);

        Debug.Log("Break");
        col.enabled = false;
        gameObject.SetActive(false);
    }
}
