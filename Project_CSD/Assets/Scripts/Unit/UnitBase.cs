using System.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum UnitState { Idle, Move, Fight, Die}
    public enum UnitActivity { Normal, Hit }

    [Header("# UnitState")]
    public UnitState unitState;
    public UnitActivity unitActivity;
    public int unitID;
    public float health;
    public float speed;
    public float power;
    public float attackTime;
}
