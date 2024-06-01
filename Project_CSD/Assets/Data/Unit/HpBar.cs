using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image realHp;
    public Transform owner;
    public GameObject parent;
    public float nowHp;
    public float maxHp;
    public Vector3 hpBarPos;
    public Vector3 hpBarDir;

    private void OnEnable()
    {
        transform.SetParent(BattleManager.Instance.hpBarParent.transform);
    }

    private void Update()
    {
        // Æ÷Áö¼Ç
        hpBarPos = new Vector3(0, -0.25f);
        transform.position = Camera.main.WorldToScreenPoint(owner.position + hpBarPos);

        // hp fillAmount
        realHp.fillAmount = (float)nowHp / (float)maxHp;

        // Color
        if (realHp.fillAmount < 0.66f)
        {
            realHp.color = Color.yellow;
        }
        else if (realHp.fillAmount < 0.33f)
        {
            realHp.color = Color.red;
        }
        else
        {
            realHp.color = Color.green;
        }
    }
}
