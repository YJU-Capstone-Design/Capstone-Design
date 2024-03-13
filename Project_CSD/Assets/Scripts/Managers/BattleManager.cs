using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleManager : Singleton<BattleManager>
{
    [Header("Shop")]
    [SerializeField] private Transform shopParent;
    [SerializeField] private GameObject card;


    [Header("HpVar")]
    [SerializeField] private float curHealth; //* ���� ü��
    [SerializeField] private float maxHealth; //* �ִ� ü��
    public GameObject healthBar; //
    public Slider HpBarSlider;

    [Header("Battle")]
    [SerializeField] private Transform targetParent;
    private void Awake()
    {

        CardMake();
        curHealth = maxHealth;
        UpdateHealthBar();
    }

    private void CardMake()
    {
        

        for(int i=0; i<3; i++)
        {
            GameObject myInstance = Instantiate(card, shopParent);
        }


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
