using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    float radius = 333f;

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
            if (i == 1)
            {
                child.localPosition = Vector2.Lerp(child.localPosition, targetPosition - new Vector2(0, 60f), Time.deltaTime * 5f);
            }
            else
            {
                child.localPosition = Vector2.Lerp(child.localPosition, targetPosition - new Vector2(30f - (30f * i), 0), Time.deltaTime * 5f);
            }
            child.localRotation = Quaternion.Euler(new Vector3(0, 0, -20 + (20 * i)));
        }
    }
}
