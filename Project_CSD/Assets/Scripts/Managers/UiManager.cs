using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : Singleton<UiManager>
{
    [Header("GameSpeed")]
    public int time = 1;
    [SerializeField] private GameObject speed_Icon;
    [SerializeField] private Sprite[] speed_IconImg;

    [Header("Cost")]
    public int cost = 0;
    private float costTime = 0f;
    private float timeInterver = 5f;
    [SerializeField] private TextMeshProUGUI costText;

    [Header("BattleUiToggle")]
    [SerializeField] private List<GameObject> battle_Btn = new List<GameObject>();
    private int toggle=0;
    [SerializeField] private GameObject[] toggleBtn;
    

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
            toggleBtn[0].SetActive(false);
            toggleBtn[1].SetActive(true);
        }
        else
        {
            foreach (GameObject go in battle_Btn)
            {
                go.SetActive(true);
                toggle = 0;
            }
            toggleBtn[0].SetActive(true);
            toggleBtn[1].SetActive(false);
        }
    }

    public void CostMgr(int cost)
    {

        
            costText.text = cost.ToString();
        
    }
    public void SpeedUp()
    {
        Image img_Icon = speed_Icon.GetComponent<Image>();
        if (time == 3)
        {
            time = 1;
            Time.timeScale = 1;
            img_Icon.sprite = speed_IconImg[0];
        }
        else
        {
            time++;
            if (time == 2) { Time.timeScale = 2; img_Icon.sprite = speed_IconImg[1]; }
            else if (time == 3) { Time.timeScale = 3; img_Icon.sprite = speed_IconImg[2]; }
        }
    }
}
