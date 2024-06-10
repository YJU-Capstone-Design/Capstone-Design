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
    [SerializeField] private TextMeshProUGUI costText; //������ �ڽ�Ʈ
    [SerializeField] private TextMeshProUGUI per_supplyText; //�ʴ� ȸ����
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
        // �����̴��� �ʱ� ����
        supply_Gage.minValue = 0;
        supply_Gage.maxValue = timeInterver; // �ִ밪�� timeInterver���� ����
        supply_Gage.value = 0; // �ʱ� ���� 0���� ����

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
                // ��� �ð��� �����մϴ�.
                elapsed += Time.deltaTime;

                // �����̴��� ���� ������Ʈ�մϴ�.
                supply_Gage.value = Mathf.Lerp(0, supply_Gage.maxValue, elapsed / timeInterver);

                // ���� �����ӱ��� ����մϴ�.
                yield return null;
            }

            // �����̴� ���� �ʱ�ȭ�մϴ�.
            supply_Gage.value = 0f;
        }
    }
}
