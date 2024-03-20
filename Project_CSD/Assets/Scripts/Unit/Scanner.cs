using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitBase;

public class Scanner : MonoBehaviour
{
    [Header("# Scanner")]
    public Vector3 scannerPos; // ��ĳ�� ��ġ
    public Vector2 scanRange; // ��ĵ ����
    public LayerMask targetLayer; // ���̾�
    public RaycastHit2D[] targets; // ��ĵ ��� �迭
    public Transform nearestTarget; // ���� ����� ��ǥ

    void FixedUpdate()
    {
        // BoxCastAll(���� ��ġ, ũ��, ȸ��, ����, ����, ��� ���̾�) : �簢���� ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ�
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
        float diff = 10; // ó�� ����� ���� �ּ� �Ÿ�

        // �νĵ� ������Ʈ���� �ÿ��̾���� �Ÿ� ���
        foreach (RaycastHit2D target in targets)
        {
            UnitBase targetLogic = target.transform.gameObject.GetComponent<UnitBase>();

            // ���� �ο�� ������ ��� �ٽ� Ž��
            if (targetLogic.unitState == UnitBase.UnitState.Fight && targetLogic.unitActivity == UnitBase.UnitActivity.Hit)
                continue;

            Vector3 myPos = transform.position; // �÷��̾� ��ġ
            Vector3 targetPos = target.transform.position; // �νĵ� ������Ʈ�� ��ġ
            float curDiff = Vector3.Distance(myPos, targetPos); // Distance(A,B) : ���� A �� B �� �Ÿ��� ������ִ� �Լ�

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        // ��� ���� �ο�� ������ �ο�� �ִ� �� �߿��� ���� ����� ������ �ٽ� Ž�� -> enemy �� ������ ����
        if (result == null && gameObject.CompareTag("PlayerUnit"))
        {
            foreach (RaycastHit2D target in targets)
            {
                Vector3 myPos = transform.position; // �÷��̾� ��ġ
                Vector3 targetPos = target.transform.position; // �νĵ� ������Ʈ�� ��ġ
                float curDiff = Vector3.Distance(myPos, targetPos); // Distance(A,B) : ���� A �� B �� �Ÿ��� ������ִ� �Լ�

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
