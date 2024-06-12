using System.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum UnitState { Idle, Move, Fight, Die, Win}

    // 데이터를 받아와서 저장하는 용도
    public float initialHealth = 0;
    public float initialSpeed = 0;
    public float initialPower = 0;
    public float initialAttackTime;

    // 실제로 적용되는 값
    [Header("# UnitState")]
    public UnitState unitState;
    public int unitID;
    public float health;
    public float speed;
    public float power;
    public float attackTime;

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
