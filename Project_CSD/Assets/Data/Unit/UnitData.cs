using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitBase;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Object/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("# Unit Type")]
    [SerializeField] // Unit Type (Friendly or Enemy)
    private UnitTypes unitType;
    public UnitTypes UnitType { get { return unitType; } }

    [Header("# Unit State")]
    [SerializeField] // ���� ���̵�
    private int unitID;
    public int UnitID { get { return unitID; } }

    [SerializeField] // Unit Name
    private string unitName;
    public string UnitName { get {  return unitName; } }

    [SerializeField] // Unit Spawn Cost
    private int cost;
    public int Cost { get { return cost; } }

    [SerializeField] // ���� ü��
    private float health;
    public float Health { get { return health; } }

    [SerializeField] // ���� ���ݷ�
    private float power;
    public float Power { get { return power; } }

    [SerializeField] // ���� �̵��ӵ�
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] // ���� ���� ��Ÿ��
    private float attackTime;
    public float AttackTime { get { return attackTime; } }
    
    [SerializeField] // ������ Ÿ�� ����||���� 1 = ���� 2 = ����
    private int item_Type;
    public float Item_Type { get { return item_Type; } }

    [SerializeField] // ���� �̹���
    private Sprite unit_Img;
    public Sprite Unit_Img { get { return unit_Img; } }

    [SerializeField] // ���� ī�� �̹���
    private Sprite unit_CardImg;
    public Sprite Unit_CardImg { get { return unit_CardImg; } }

    [SerializeField] // ���� Skeleton Data (Spine)
    private SkeletonDataAsset unit_skeletonData;
    public SkeletonDataAsset Unit_skeletonData { get { return unit_skeletonData; } }

    [SerializeField] // ���� ���� Ÿ��
    private Sprite unit_Atk_Type;
    public Sprite Unit_Atk_Type { get { return unit_Atk_Type; } }

    //[Header("# Level State")]

    //[SerializeField] // ������ ���� ü�� ������
    //private float healths;
    //public float Healths { get { return healths; } }

    //[SerializeField] // ������ ���� ���ݷ� ������
    //private float powers;
    //public float Powers { get { return powers; } }
}
