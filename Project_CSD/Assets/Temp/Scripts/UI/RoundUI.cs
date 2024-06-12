using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    float radius = 333f;

    void Update()
    {
        int numOfObjects = 3; // 배치할 자손 오브젝트의 수
        float angleIncrement = 180f / (numOfObjects + 1); // 반원 내에서 각 자손 오브젝트의 간격

        for (int i = 0; i < numOfObjects; i++)
        {
            float angle = (i + 1) * angleIncrement; // 반원 내에서 자손 오브젝트의 각도

            Transform child = transform.GetChild(i); // i번째 자손 오브젝트 가져오기

            // 각도를 라디안으로 변환하여 코사인과 사인 값을 계산하여 위치를 설정합니다.
            float radianAngle = angle * Mathf.Deg2Rad;
            Vector2 targetPosition = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * radius;

            // 보간을 사용하여 부드럽게 이동합니다.
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
