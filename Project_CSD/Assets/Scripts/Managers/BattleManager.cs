using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleManager :Singleton<BattleManager>
{
    [Header("HpVar")]
    public float curHealth; //* 현재 체력
    public float maxHealth; //* 최대 체력
    public GameObject healthBar; //
    public Slider HpBarSlider;

    private void Awake()
    {
        curHealth = maxHealth;
        UpdateHealthBar();
    }
    
    public void HpDamage(float dmg)
    {
        float damage = dmg;
        curHealth -= damage;
        UpdateHealthBar();
    }
    void UpdateHealthBar()
    {
        
        float sliderValue = curHealth / maxHealth;
        HpBarSlider.value = sliderValue;
        if (curHealth <= 0)
        {
            healthBar.SetActive(false);
        }
    }
}
