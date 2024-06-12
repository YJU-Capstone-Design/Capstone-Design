using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitBase;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("# Main State")]
    [SerializeField] // ���� ���̵�
    private int unitID;
    public int UnitID { get { return unitID; } }

    [SerializeField] // ���� ü��
    private float health;
    public float Health { get { return health; } }

    [SerializeField] // ���� �̵��ӵ�
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField] // ���� ���ݷ�
    private float power;
    public float Power { get { return power; } }

    [SerializeField] // ���� ���� ��Ÿ��
    private float attackTime;
    public float AttackTime { get { return attackTime; } }


    //[Header("# Level State")]

    //[SerializeField] // ������ ���� ü�� ������
    //private float healths;
    //public float Healths { get { return healths; } }

    //[SerializeField] // ������ ���� ���ݷ� ������
    //private float powers;
    //public float Powers { get { return powers; } }
}
