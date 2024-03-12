using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour
{
    [Header("HpVar")]
    [SerializeField] private float curHealth; //* 현재 체력
    [SerializeField] private float maxHealth; //* 최대 체력
    public Slider HpBarSlider;

    private void Awake()
    {
        curHealth = maxHealth;
    }
    private void Update()
    {
        Invoke("HpCheck", 3f);
    }
    public void HpCheck()
    {
        curHealth -= 3f;
        HpBarSlider.value = curHealth;
    }

}
