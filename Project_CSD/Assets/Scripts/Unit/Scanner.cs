using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; // ��ĵ ����
    public LayerMask targetLayer; // ���̾�
    public RaycastHit2D[] targets; // ��ĵ ��� �迭
    public Transform nearestTarget; // ���� ����� ��ǥ

    void FixedUpdate()
    {
        // CircleCastAll(���� ��ġ, ������, ĳ���� ����, ĳ���� ����, ��� ���̾�) : ������ ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ�
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 10; // ó�� ����� ���� �ּ� �Ÿ�

        // �νĵ� ������Ʈ���� �ÿ��̾���� �Ÿ� ���
        foreach (RaycastHit2D target in targets)
        {
            UnitBase targetLogic = target.transform.gameObject.GetComponent<UnitBase>();

            if (targetLogic.unitState == UnitBase.UnitState.Fight)
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

        // ��� ���� �ο�� ������ �ο�� �ִ� �� �߿��� ���� ����� ������ �ٽ� Ž��
        if(result == null)
        {
            foreach (RaycastHit2D target in targets)
            {
                UnitBase targetLogic = target.transform.gameObject.GetComponent<UnitBase>();

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
