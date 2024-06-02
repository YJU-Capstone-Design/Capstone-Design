using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BreadRack", menuName = "Scriptable Object/BreadRack_Info")]
public class BreadRack_Data : ScriptableObject {

    [Header("# Main State")] //진열대 아이템
    [SerializeField]  // 이름
    private string breadRack_Name;
    public string BreadRack_Name { get { return breadRack_Name; } }

    [SerializeField]  // 레어도 타입
    private int breadRack_RareType;
    public int BreadRack_RareType { get { return breadRack_RareType; } }

    [SerializeField] // 번호 번호 가장 앞자리로 타입 구분 1001 2001 1=파워 2= 속도 가장 뒷자리는 개인번호
    private int breadRack_No;
    public int BreadRack_No { get { return breadRack_No; } }

    [SerializeField] // 코스트
    private int breadRack_Cost;
    public int BreadRack_Cost { get { return breadRack_Cost; } }

    [SerializeField] // 레벨 레벨로 high카드인지 구분
    private int breadRack_Level;
    public int BreadRack_Level { get { return breadRack_Level; } }

    [SerializeField] // 이미지
    private Sprite breadRack_Img;
    public Sprite BreadRack_Img { get { return breadRack_Img; } }

    [SerializeField] // 증감 스텟
    private float breadRack_Stats;
    public float BreadRack_Stats { get { return breadRack_Stats; } }

    [SerializeField] // 설명서
    private string breadRack_InfoTxt;
    public string BreadRack_InfoTxt { get { return breadRack_InfoTxt; } }

}
