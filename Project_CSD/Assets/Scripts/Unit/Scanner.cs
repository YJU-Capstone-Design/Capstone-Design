using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitBase;

public class Scanner : MonoBehaviour
{
    [Header("# Scanner")]
    public Vector3 scannerPos; // 스캐너 위치
    public Vector2 scanRange; // 스캔 범위
    public LayerMask targetLayer; // 레이어
    public RaycastHit2D[] targets; // 스캔 결과 배열
    public Transform nearestTarget; // 가장 가까운 목표

    void FixedUpdate()
    {
        // BoxCastAll(시작 위치, 크기, 회전, 방향, 길이, 대상 레이어) : 사각형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        targets = Physics2D.BoxCastAll(transform.position + scannerPos, scanRange, 0, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + scannerPos, scanRange);
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 10; // 처음 계산을 위한 최소 거리

        // 인식된 오브젝트마다 플에이어와의 거리 계산
        foreach (RaycastHit2D target in targets)
        {
            UnitBase targetLogic = target.transform.gameObject.GetComponent<UnitBase>();

            // 적이 싸우는 상태일 경우 다시 탐색
            if (targetLogic.unitState == UnitBase.UnitState.Fight && targetLogic.unitActivity == UnitBase.UnitActivity.Hit)
                continue;

            Vector3 myPos = transform.position; // 플레이어 위치
            Vector3 targetPos = target.transform.position; // 인식된 오브젝트의 위치
            float curDiff = Vector3.Distance(myPos, targetPos); // Distance(A,B) : 벡터 A 와 B 의 거리를 계산해주는 함수

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        // 모든 적이 싸우고 있으면 싸우고 있는 적 중에서 가장 가까운 적으로 다시 탐색 -> enemy 는 벽으로 직진
        if (result == null && gameObject.CompareTag("PlayerUnit"))
        {
            foreach (RaycastHit2D target in targets)
            {
                Vector3 myPos = transform.position; // 플레이어 위치
                Vector3 targetPos = target.transform.position; // 인식된 오브젝트의 위치
                float curDiff = Vector3.Distance(myPos, targetPos); // Distance(A,B) : 벡터 A 와 B 의 거리를 계산해주는 함수

                if (curDiff < diff)
                {
                    diff = curDiff;
                    result = target.transform;
                }
            }
        }

        return result;
    }
}
