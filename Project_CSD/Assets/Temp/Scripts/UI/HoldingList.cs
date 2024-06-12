using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingList : MonoBehaviour
{
    public static HoldingList single { get; private set; }
    public List<UnitData> holding_Unit = new List<UnitData>();
    public List<SpellData> Cardholding_Spell = new List<SpellData>();
    public void Awake()
    {
        single = this;
    }
    public void Update_Hoding(UnitData data)
    {
        Debug.Log(data.UnitName);
        int newid = data.UnitID;
        bool exists = false;

        // ����Ʈ�� ������ ID�� ���� ������ �ִ��� Ȯ��
        for (int i = 0; i < holding_Unit.Count; i++)
        {
            if (holding_Unit[i].UnitID == newid)
            {
                exists = true;
                break;
            }
        }

        // ������ ID�� ���� ������ ���� ��쿡�� ����Ʈ�� �߰�
        if (!exists)
        {
            holding_Unit.Add(data);
            Debug.Log("Added unit: " + data.UnitName);
        }
        else
        {
            Debug.Log("Unit already exists: " + data.UnitName);
        }
    }









    public void Update_SpellHoding(SpellData data)
    {
        Debug.Log(data.SpellName);
        int newid = data.SpellID;
        bool exists = false;

        // ����Ʈ�� ������ ID�� ���� ������ �ִ��� Ȯ��
        for (int i = 0; i < Cardholding_Spell.Count; i++)
        {
            if (Cardholding_Spell[i].SpellID == newid)
            {
                exists = true;
                break;
            }
        }

        // ������ ID�� ���� ������ ���� ��쿡�� ����Ʈ�� �߰�
        if (!exists)
        {
            Cardholding_Spell.Add(data);
            Debug.Log("Added spell: " + data.SpellName);
        }
        else
        {
            Debug.Log("Spell already exists: " + data.SpellName);
        }
    }
}
