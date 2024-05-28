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
        switch (owner.gameObject.name)
        {
            case string name when name.Contains("Cyclope") || name.Contains("Orc") || name.Contains("Orc"):
                hpBarPos = new Vector3((hpBarDir.x < 0 ? -0.2f : 0f), 3.5f);
                break;
            case string name when name.Contains("Skeleton") || name.Contains("Zombie"):
                hpBarPos = new Vector3((hpBarDir.x < 0 ? -0.1f : 0f), 1.8f);
                break;
            case string name when name.Contains("Slime"):
                hpBarPos = new Vector3((hpBarDir.x < 0 ? 0f : -0.1f), 1.3f);
                break;
            case string name when name.Contains("WhiteBread"):
                hpBarPos = new Vector3((hpBarDir.x < 0 ? -0.2f : 0f), 2f);
                break;
            case string name when name.Contains("Ramo"):
                hpBarPos = new Vector3((hpBarDir.x < 0 ? -0.4f : -0.1f), 1.8f);
                break;
            case string name when name.Contains("Pupnut"):
                hpBarPos = new Vector3(-0.05f, 1.5f);
                break;
            default:
                hpBarPos = new Vector3((hpBarDir.x < 0 ? -0.2f : 0f), 1.5f);
                break;
        }

        // ������
        transform.position = Camera.main.WorldToScreenPoint(owner.position + hpBarPos);

        // hp fillAmount
        realHp.fillAmount = (float)nowHp / (float)maxHp;

        // Color
        if(realHp.fillAmount < 0.66f)
        {
            realHp.color = Color.yellow;
        } else if(realHp.fillAmount < 0.33f)
        {
            realHp.color = Color.red;
        } else
        {
            realHp.color = Color.green;
        }
    }
}
