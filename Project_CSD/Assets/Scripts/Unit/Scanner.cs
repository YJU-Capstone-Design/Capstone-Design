using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;
using static UnitBase;
using static UnityEngine.GraphicsBuffer;

public class Scanner : MonoBehaviour
{
    [Header("# Scanner")]
    public Vector3 scannerPos; // ��ĳ�� ��ġ
    public Vector2 scanRange; // ��ĵ ����
    public LayerMask targetLayer; // ���̾�
    public RaycastHit2D[] targets; // ��ĵ ��� �迭
    public Transform nearestTarget; // ���� ����� ��ǥ
    public int unitType;

    void Update()
    {
        // BoxCastAll(���� ��ġ, ũ��, ȸ��, ����, ����, ��� ���̾�) : �簢���� ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ�
        targets = Physics2D.BoxCastAll(transform.position + scannerPos, scanRange, 0, Vector2.zero, 0, targetLayer);

        nearestTarget = GetNearest(targets);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + scannerPos, scanRange);
    }

    // �迭 �� ���� ����� ���� ã�� �Լ� (�̵�)
    public Transform GetNearest(RaycastHit2D[] targets)
    {
        Transform result = null;
        float diff = 20; // ó�� ����� ���� �ּ� �Ÿ�

        // �νĵ� ������Ʈ���� �ÿ��̾���� �Ÿ� ���
        foreach (RaycastHit2D target in targets)
        {
            // ���� �ο�� ������ ��� �ٽ� Ž�� -> �پ��ϰ� ���� ��Ű�� ���� ����
            if(unitType == 1)
            {
                EnemyUnit targetLogic = target.transform.gameObject.GetComponent<EnemyUnit>();
                if (targetLogic.unitState == UnitBase.UnitState.Fight)
                {
                    continue;
                } else if(targetLogic.scanner.nearestTarget != null)
                {
                    if(targetLogic.scanner.nearestTarget != this.gameObject.transform)
                    {
                        continue;
                    }
                }
            } 
            else if(unitType == 3)
            {
                PlayerUnit targetLogic = target.transform.gameObject.GetComponent<PlayerUnit>();
                if (targetLogic.unitState == UnitBase.UnitState.Fight)
                {
                    continue;
                }
                else if (targetLogic.scanner.nearestTarget != null)
                {
                    if (targetLogic.scanner.nearestTarget != this.gameObject.transform)
                    {
                        continue;
                    }
                }
            }

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
        if (result == null && unitType == 1)
        {
            result = GetNearestAttack(targets);
        }

        return result;
    }

    // �迭 �� ���� ����� ���� ã�� �Լ� (����)
    public Transform GetNearestAttack(RaycastHit2D[] targets)
    {
        Transform result = null;
        float diff = 20; // ó�� ����� ���� �ּ� �Ÿ�

        // �νĵ� ������Ʈ���� �ÿ��̾���� �Ÿ� ���
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
        return result;
    }

    // �ټ� ���� ��ǥ ã��
    public Transform[] GetAttackTargets(RaycastHit2D[] targets, int count)
    {
        Transform[] results = null;

        if(targets.Length < count)
        {
            results = new Transform[targets.Length];

            for (int i = 0; i < targets.Length; i++)
            {
                results[i] = targets[i].transform;
            }

        } else
        {
            results = new Transform[count];

            for (int i = 0; i < count; i++)
            {
                results[i] = targets[i].transform;
            }
        }

        return results;
    }
}
