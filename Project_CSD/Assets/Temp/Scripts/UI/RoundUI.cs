using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    float radius = 360f;
    private int numOfObjects = 3;
    private bool isReady = false;

    private void OnEnable()
    {
        isReady = false;
        StartCoroutine(InitAfterFrame());
    }

    // 한 프레임 대기 후 자식 위치 리셋
    // (Instantiate 직후엔 자식이 아직 레이아웃에 안 잡혀있어서)
    IEnumerator InitAfterFrame()
    {
        yield return null; // 1프레임 대기

        for (int i = 0; i < numOfObjects; i++)
        {
            if (i < transform.childCount)
                transform.GetChild(i).localPosition = Vector2.zero;
        }
        isReady = true;
    }

    void Update()
    {
        // 초기화 완료 전엔 실행 안 함
        if (!isReady) return;

        float angleIncrement = 180f / (numOfObjects + 1);
        bool allArrived = true;

        for (int i = 0; i < numOfObjects; i++)
        {
            if (i >= transform.childCount) break;

            float angle = (i + 1) * angleIncrement;
            Transform child = transform.GetChild(i);

            float radianAngle = angle * Mathf.Deg2Rad;
            Vector2 targetPosition = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)) * radius;

            if (i == 1)
                targetPosition -= new Vector2(0, 60f);
            else
                targetPosition -= new Vector2(20f - (20f * i), 0);

            child.localPosition = Vector2.Lerp(child.localPosition, targetPosition, Time.deltaTime * 5f);
            child.localRotation = Quaternion.Euler(new Vector3(0, 0, -22.5f + (22.5f * i)));

            if (Vector2.Distance(child.localPosition, targetPosition) > 0.1f)
                allArrived = false;
            else
                child.localPosition = targetPosition;
        }

        if (allArrived)
            this.enabled = false;
    }

    // CardMake() 이후 외부에서 호출
    public void ReActivate()
    {
        StopAllCoroutines();
        isReady = false;
        this.enabled = true;
        StartCoroutine(InitAfterFrame());
    }
}