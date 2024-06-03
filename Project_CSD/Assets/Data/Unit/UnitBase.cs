using System.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum UnitTypes { Friendly, Enemy }
    public enum UnitState { Idle, Move, Fight, Die, Win }

    // 데이터를 받아와서 저장하는 용도
    public float initialHealth;
    public float initialMoveSpeed;
    public float initialPower;
    public float initialAttackSpeed;

    [Header("# Unit Type")]
    public UnitTypes unitType;

    // 실제로 적용되는 값
    [Header("# Unit State")]
    public UnitState unitState;

    public int unitID;
    public string unitName;

    public int cost;

    public float health;
    public float power;
    public float attackSpeed;
    public float moveSpeed;

    // 스프라이트 방향 설정 함수
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
