using System.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum UnitState { Idle, Move, Fight, Die, Win}

    // �����͸� �޾ƿͼ� �����ϴ� �뵵
    public float initialHealth = 0;
    public float initialSpeed = 0;
    public float initialPower = 0;
    public float initialAttackTime;

    // ������ ����Ǵ� ��
    [Header("# UnitState")]
    public UnitState unitState;
    public int unitID;
    public float health;
    public float speed;
    public float power;
    public float attackTime;

    // ��������Ʈ ���� ���� �Լ�
    protected void SpriteDir(Vector3 firstVec, Vector3 secondVec)
    {
        if (firstVec.x > secondVec.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (firstVec.x < secondVec.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
