using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    float radius = 320f;

    void Update()
    {
        int numOfObjects = 3; // ��ġ�� �ڼ� ������Ʈ�� ��
        float angleIncrement = 180f / (numOfObjects + 1); // �ݿ� ������ �� �ڼ� ������Ʈ�� ����

        for (int i = 0; i < numOfObjects; i++)
        {
            float angle = (i + 1) * angleIncrement; // �ݿ� ������ �ڼ� ������Ʈ�� ����

            Transform child = transform.GetChild(i); // i��° �ڼ� ������Ʈ ��������

            // ������ �������� ��ȯ�Ͽ� �ڻ��ΰ� ���� ���� ����Ͽ� ��ġ�� �����մϴ�.
            float radianAngle = angle * Mathf.Deg2Rad;
            Vector2 targetPosition = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * radius;

            // ������ ����Ͽ� �ε巴�� �̵��մϴ�.
            child.localPosition = Vector2.Lerp(child.localPosition, targetPosition, Time.deltaTime * 5f);
        }
    }
}
