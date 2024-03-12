using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour
{
    [Header("HpVar")]
    [SerializeField] private float curHealth; //* ���� ü��
    [SerializeField] private float maxHealth; //* �ִ� ü��
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
