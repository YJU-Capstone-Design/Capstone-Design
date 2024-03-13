using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour
{

    public static BattleManager instance;
    [Header("HpVar")]
    [SerializeField] private float curHealth; //* ���� ü��
    [SerializeField] private float maxHealth; //* �ִ� ü��
    public GameObject healthBar; //
    public Slider HpBarSlider;

    private void Awake()
    {
        instance = this;
        curHealth = maxHealth;
        UpdateHealthBar();
    }
    
    public void HpDamage()
    {
        float damage = 300f;
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
