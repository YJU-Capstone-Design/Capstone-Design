using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : Singleton<UiManager>
{
    [Header("GameSpeed")]
    private int time = 1;
    [SerializeField] private TextMeshProUGUI gameSpeed;

    [Header("Cost")]
    public int cost = 0;
    private float costTime = 0f;
    private float timeInterver = 5f;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("BattleUiToggle")]
    [SerializeField] private List<GameObject> battle_Btn = new List<GameObject>();
    private int toggle=0;

    private void Awake()
    {
        cost = 10;
        costText.text = cost.ToString();
    }
    private void Update()
    {
        costTime += Time.deltaTime;
        if (costTime >= timeInterver)
        {
            costTime = 0f;
            cost += 2;
            CostMgr(cost);
        }
        
    }

    public void CloseToggle()
    {
        if (toggle == 0)
        {
            foreach(GameObject go in battle_Btn)
            {
                go.SetActive(false);
                toggle = 1;
            }

        }
        else
        {
            foreach (GameObject go in battle_Btn)
            {
                go.SetActive(true);
                toggle = 0;
            }
        }
    }

    public void CostMgr(int cost)
    {

        
            costText.text = cost.ToString();
        
    }
    public void SpeedUp()
    {
        if (time == 3)
        {
            time = 1;
            Time.timeScale = 1;
            gameSpeed.text = "1X";
        }
        else
        {
            time++;
            if (time == 2) { Time.timeScale = 2; gameSpeed.text = "2X"; }
            else if (time == 3) { Time.timeScale = 3; gameSpeed.text = "3X"; }
        }
    }
}
