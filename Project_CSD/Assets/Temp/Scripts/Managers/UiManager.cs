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
    public float costTime = 0f;
    public float timeInterver = 1f;
    [SerializeField] private TextMeshProUGUI costText; //보유한 코스트
    [SerializeField] private TextMeshProUGUI per_supplyText; //초당 회복률
    [SerializeField] private Slider supply_Gage;

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
    private void Start()
    {
        // 슬라이더의 초기 설정
        supply_Gage.minValue = 0;
        supply_Gage.maxValue = timeInterver; // 최대값을 timeInterver으로 설정
        supply_Gage.value = 0; // 초기 값을 0으로 설정

        StartCoroutine(FillSlider());
    }

    private void Update()
    {

        per_supplyText.text = supply.ToString() + " / " + timeInterver.ToString() + "s";

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
    private IEnumerator FillSlider()
    {
        while (true)
        {
            float elapsed = 0f;

            while (elapsed < timeInterver)
            {
                // 경과 시간을 누적합니다.
                elapsed += Time.deltaTime;

                // 슬라이더의 값을 업데이트합니다.
                supply_Gage.value = Mathf.Lerp(0, supply_Gage.maxValue, elapsed / timeInterver);

                // 다음 프레임까지 대기합니다.
                yield return null;
            }

            // 슬라이더 값을 초기화합니다.
            supply_Gage.value = 0f;
        }
    }
}
