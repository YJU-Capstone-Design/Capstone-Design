using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BreadRack", menuName = "Scriptable Object/BreadRack_Info")]
public class BreadRack_Data : ScriptableObject {

    [Header("# Main State")] //������ ������
    [SerializeField]  // �̸�
    private string breadRack_Name;
    public string BreadRack_Name { get { return breadRack_Name; } }

    [SerializeField]  // ��� Ÿ��
    private int breadRack_RareType;
    public int BreadRack_RareType { get { return breadRack_RareType; } }

    [SerializeField] // ��ȣ ��ȣ ���� ���ڸ��� Ÿ�� ���� 1001 2001 1=�Ŀ� 2= �ӵ� ���� ���ڸ��� ���ι�ȣ
    private int breadRack_No;
    public int BreadRack_No { get { return breadRack_No; } }

    [SerializeField] // �ڽ�Ʈ
    private int breadRack_Cost;
    public int BreadRack_Cost { get { return breadRack_Cost; } }

    [SerializeField] // ���� ������ highī������ ����
    private int breadRack_Level;
    public int BreadRack_Level { get { return breadRack_Level; } }

    [SerializeField] // �̹���
    private Sprite breadRack_Img;
    public Sprite BreadRack_Img { get { return breadRack_Img; } }

    [SerializeField] // ���� ����
    private float breadRack_Stats;
    public float BreadRack_Stats { get { return breadRack_Stats; } }

    [SerializeField] // ����
    private string breadRack_InfoTxt;
    public string BreadRack_InfoTxt { get { return breadRack_InfoTxt; } }

}
