using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public GameObject infomation;
    public GameObject icon;
    public AudioSource audioSource;
    public AudioClip clickSound;

    [Header("Jump Settings")]
    public float jumpDuration = 0.5f; // 튀어오르는 총 시간
    public float jumpHeight = 150f;   // 위로 솟구치는 높이
    public float sideRange = 50f;     // 옆으로 랜덤하게 퍼지는 범위

    private Vector3 originPosition;   // 원래 위치 저장용
    private bool isJumping = false;   // 중복 클릭 방지

    void Start()
    {
        // 시작할 때 아이콘의 초기 위치를 저장합니다.
        if (icon != null) originPosition = icon.transform.localPosition;
    }

    public void OpenInfo() => infomation.SetActive(true);
    public void CloseInfo() => infomation.SetActive(false);

    public void PopUpAndPlaySound()
    {
        if (isJumping) return; // 이미 뛰고 있다면 무시

        // 1. 소리 재생
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        // 2. 튀기기 연출 시작
        StartCoroutine(JumpRoutine());
    }

    IEnumerator JumpRoutine()
    {
        isJumping = true;
        float elapsed = 0f;

        // 랜덤한 좌우 목표 지점 설정 (부채꼴 느낌)
        float randomX = Random.Range(-sideRange, sideRange);
        Vector3 targetPos = originPosition + new Vector3(randomX, jumpHeight, 0);

        // [상승 단계] 원래 위치 -> 랜덤 정점
        while (elapsed < jumpDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (jumpDuration / 2);
            // 쿼드라틱 아웃(Smooth Step) 효과로 부드럽게 상승
            icon.transform.localPosition = Vector3.Lerp(originPosition, targetPos, Mathf.Sin(t * Mathf.PI * 0.5f));
            yield return null;
        }

        elapsed = 0f;

        // [하강 단계] 정점 -> 원래 위치
        while (elapsed < jumpDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (jumpDuration / 2);
            // 가속도를 붙여서 하강
            icon.transform.localPosition = Vector3.Lerp(targetPos, originPosition, t * t);
            yield return null;
        }

        // 정확히 원래 위치로 고정
        icon.transform.localPosition = originPosition;
        isJumping = false;
    }
}