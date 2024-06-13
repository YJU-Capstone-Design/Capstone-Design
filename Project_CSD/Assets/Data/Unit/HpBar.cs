using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Transform realHp;
    public SpriteRenderer realHpSprite;
    public SpriteRenderer hpFrameSprite;
    public Transform owner;
    public float nowHp;
    public float maxHp;
    public Vector3 hpBarPos;

    private void Update()
    {
        // Æ÷Áö¼Ç
        hpBarPos = new Vector3(-0.3f, -0.25f);
        transform.position = owner.position + hpBarPos;

        // hp fillAmount (Scale)
        realHp.localScale = new Vector3((float)nowHp / (float)maxHp, 1, 1);

        // Color
        if (realHp.localScale.x > 0.25f && realHp.localScale.x <= 0.75f)
        {
            realHpSprite.color = Color.yellow;
        }
        else if (realHp.localScale.x <= 0.25f)
        {
            realHpSprite.color = Color.red;
        }
        else
        {
            realHpSprite.color = Color.green;
        }
    }
}
