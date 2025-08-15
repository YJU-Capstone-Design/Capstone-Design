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
    [SerializeField] // 유닛 아이디
    private int unitID;
    public int UnitID { get { return unitID; } }

    [SerializeField] // Unit Name
    private string unitName;
    public string UnitName { get {  return unitName; } }

    [SerializeField] // Unit Spawn Cost
    private int cost;
    public int Cost { get { return cost; } }

    [SerializeField] // 유닛 체력
    private float health;
    public float Health { get { return health; } }

    [SerializeField] // 유닛 공격력
    private float power;
    public float Power { get { return power; } }

    [SerializeField] // 유닛 이동속도
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] // 유닛 공격 쿨타임
    private float attackTime;
    public float AttackTime { get { return attackTime; } }
    
    [SerializeField] // 아이템 타입 유닛||스펠 1 = 유닛 2 = 스펭
    private int item_Type;
    public float Item_Type { get { return item_Type; } }

    [SerializeField] // 유닛 이미지
    private Sprite unit_Img;
    public Sprite Unit_Img { get { return unit_Img; } }

    [SerializeField] // 유닛 카드 이미지
    private Sprite unit_CardImg;
    public Sprite Unit_CardImg { get { return unit_CardImg; } }

    [SerializeField] // 유닛 Skeleton Data (Spine)
    private SkeletonDataAsset unit_skeletonData;
    public SkeletonDataAsset Unit_skeletonData { get { return unit_skeletonData; } }

    [SerializeField] // 유닛 공격 타입
    private Sprite unit_Atk_Type;
    public Sprite Unit_Atk_Type { get { return unit_Atk_Type; } }

    //[Header("# Level State")]

    //[SerializeField] // 레벨에 따른 체력 증가량
    //private float healths;
    //public float Healths { get { return healths; } }

    //[SerializeField] // 레벨에 따른 공격력 증가량
    //private float powers;
    //public float Powers { get { return powers; } }
}
