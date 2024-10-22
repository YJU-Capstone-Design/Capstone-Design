using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemInfo : MonoBehaviour
{
    public static ItemInfo instance;
    [SerializeField] TextMeshProUGUI[] unititem;

    [SerializeField] TextMeshProUGUI[] spellitem;

    public Image collImg;
    [SerializeField] GameObject collInfo;

    private void Start()
    {
        instance = this;
      
       
    }
    private void OnEnable()
    {
        InfoClear();
    }
    public void InfoClear()
    {
        for (int i = 0; i < unititem.Length; i++)
        {
            unititem[i].text = "";
        }
        for (int i = 0; i < spellitem.Length; i++)
        {
            spellitem[i].text = "";
        }
    }
    public void UnitCollectionInfo(UnitData data)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        collInfo.SetActive(true);
        collImg.sprite = data.Unit_CardImg;
        OpenInfoUnit(data);
    }
    public void SpellCollectionInfo(SpellData data)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        collInfo.SetActive(true);
        collImg.sprite = data.Spell_CardImg;
        OpenInfoSpell(data);
    }
    public void CollectionInfoCancell()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        collInfo.SetActive(false);
    }


    public void OpenInfoUnit(UnitData data)
    {
        InfoClear();

        unititem[0].text = data.UnitName;
        unititem[1].text =  data.Cost.ToString();
        unititem[2].text = "체력 " + data.Health.ToString();
        unititem[3].text = "공격력 " + data.Power.ToString();
        unititem[4].text = "스피드 " + data.MoveSpeed.ToString();
        unititem[5].text = "공격 속도 " + data.AttackTime.ToString();
    }

    public void OpenInfoSpell(SpellData data)
    {
        InfoClear();

        spellitem[0].text =data.SpellName;
        spellitem[1].text = data.Cost.ToString();
        spellitem[2].text = "지속시간 "+data.Duration.ToString();
        if (data.SpellID == 23002)
        {
            spellitem[3].text = "적 유닛 일시 정지 ";
        }
       else if(data.SpellType == SpellData.SpellTypes.Debuff && data.SpellID != 23002) {

            if (data.PowerUp != 0 && spellitem[3].text.Equals("")) { spellitem[3].text = "적 공격력 감소 " + data.PowerUp.ToString() + "%"; }
            else if (data.PowerUp != 0 && spellitem[4].text.Equals("")) { spellitem[4].text = "적 공격력 감소 " + data.PowerUp.ToString() +"%"; }
            else if (data.PowerUp != 0 && spellitem[5].text.Equals("")) { spellitem[5].text = "적 공격력 감소 " + data.PowerUp.ToString() +"%"; }

            if (data.MaxHpUp != 0 && spellitem[3].text.Equals("")) { spellitem[3].text = "적 최대 체력 감소 " + data.MaxHpUp.ToString() + "%"; }
            else if (data.MaxHpUp != 0 && spellitem[4].text.Equals("")) { spellitem[4].text = "적 최대체력 감소 " + data.MaxHpUp.ToString() + "%"; }
            else if (data.MaxHpUp != 0 && spellitem[5].text.Equals("")) { spellitem[5].text = "적 최대체력 감소 " + data.MaxHpUp.ToString() + "%"; }

            if (data.MoveSpeedUp != 0 && spellitem[3].text.Equals("")) { spellitem[3].text = "적 이동속도 감소 " + data.MoveSpeedUp.ToString() + "%"; }
            else if (data.MoveSpeedUp != 0 && spellitem[4].text.Equals("")) { spellitem[4].text = "적 이동 속도 감소 " + data.MoveSpeedUp.ToString() + "%"; }
            else if (data.MoveSpeedUp != 0 && spellitem[5].text.Equals("")) { spellitem[5].text = "적 이동 속도 감소 " + data.MoveSpeedUp.ToString() + "%"; }

            if (data.AttackTimeDown != 0 && spellitem[3].text.Equals("")) { spellitem[3].text = "적 공격 속도 감소 " + data.AttackTimeDown.ToString() + "%"; }
            else if (data.AttackTimeDown != 0 && spellitem[4].text.Equals("")) { spellitem[4].text = "적 공격 속도 감소 " + data.AttackTimeDown.ToString() + "%"; }
            else if (data.AttackTimeDown != 0 && spellitem[5].text.Equals("")) { spellitem[5].text = "적 공격 속도 감소 " + data.AttackTimeDown.ToString() + "%"; }
        }
        else
        {
            if (data.PowerUp != 0 && spellitem[3].text.Equals("")) { spellitem[3].text = "공격력 증가 " + data.PowerUp.ToString() + "%"; }
            else if (data.PowerUp != 0 && spellitem[4].text.Equals("")) { spellitem[4].text = "공격력 증가 " + data.PowerUp.ToString() + "%"; }
            else if (data.PowerUp != 0 && spellitem[5].text.Equals("")) { spellitem[5].text = "공격력 증가 " + data.PowerUp.ToString() + "%"; }

            if (data.MaxHpUp != 0 && spellitem[3].text.Equals("")) { spellitem[3].text = "최대 체력 증가 " + data.MaxHpUp.ToString() + "%"; }
            else if (data.MaxHpUp != 0 && spellitem[4].text.Equals("")) { spellitem[4].text = "최대 체력 증가 " + data.MaxHpUp.ToString() + "%"; }
            else if (data.MaxHpUp != 0 && spellitem[5].text.Equals("")) { spellitem[5].text = "최대 체력 증가 " + data.MaxHpUp.ToString() + "%"; }

            if (data.MoveSpeedUp != 0 && spellitem[3].text.Equals("")) { spellitem[3].text = "이동속도 증가 " + data.MoveSpeedUp.ToString() + "%"; }
            else if (data.MoveSpeedUp != 0 && spellitem[4].text.Equals("")) { spellitem[4].text = "이동 속도 증가 " + data.MoveSpeedUp.ToString() + "%"; }
            else if (data.MoveSpeedUp != 0 && spellitem[5].text.Equals("")) { spellitem[5].text = "이동 속도 증가 " + data.MoveSpeedUp.ToString() + "%"; }

            if (data.AttackTimeDown != 0 && spellitem[3].text.Equals("")) { spellitem[3].text = "공격 속도 증가 " + data.AttackTimeDown.ToString() + "%"; }
            else if (data.AttackTimeDown != 0 && spellitem[4].text.Equals("")) { spellitem[4].text = "공격 속도 증가 " + data.AttackTimeDown.ToString() + "%"; }
            else if (data.AttackTimeDown != 0 && spellitem[5].text.Equals("")) { spellitem[5].text = "공격 속도 증가 " + data.AttackTimeDown.ToString() + "%"; }


        }
        
       
    }
}
