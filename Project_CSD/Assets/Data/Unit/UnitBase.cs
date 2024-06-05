using System.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum UnitTypes { Friendly, Enemy }
    public enum UnitState { Idle, Move, Fight, Die, Win }

    // �����͸� �޾ƿͼ� �����ϴ� �뵵
    public float initialHealth;
    public float initialMoveSpeed;
    public float initialPower;
    public float initialAttackSpeed;

    [Header("# Unit Type")]
    public UnitTypes unitType;

    // ������ ����Ǵ� ��
    [Header("# Unit State")]
    public UnitState unitState;

    public int unitID;
    public string unitName;

    public int cost;

    public float health;
    public float power;
    public float attackSpeed;
    public float moveSpeed;

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
