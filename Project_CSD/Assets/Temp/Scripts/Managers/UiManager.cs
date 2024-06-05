using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : Singleton<UiManager>
{
    public UiManager instance;
    [Header("GameSpeed")]
    public int time = 1;
    [SerializeField] private GameObject speed_Icon;
    [SerializeField] private Sprite[] speed_IconImg;

    [Header("Cost")]
    public int cost = 0;
    public int supply;
    private float costTime = 0f;
    private float timeInterver = 5f;
    [SerializeField] private TextMeshProUGUI costText; //보유한 코스트
    [SerializeField] private TextMeshProUGUI per_costText; //몇초당 회복
    [SerializeField] private TextMeshProUGUI per_supplyText; //초당 회복률

    [Header("BattleUiToggle")]
    [SerializeField] private List<GameObject> battle_Btn = new List<GameObject>();
    private int toggle=0;
    [SerializeField] private GameObject[] toggleBtn;

    private void Awake()
    {
        instance = this;

        cost = 0;
        costText.text = cost.ToString();
    }
    private void Update()
    {
        per_costText.text = timeInterver.ToString();
        per_supplyText.text = supply.ToString();

        costTime += Time.deltaTime;
        if (costTime >= timeInterver)
        {
            costTime = 0f;
            cost += supply;
        }
        CostMgr(cost);
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
