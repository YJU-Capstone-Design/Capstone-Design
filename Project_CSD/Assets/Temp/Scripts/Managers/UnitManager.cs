using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private UnitData unitData;

    Unit unit;
    private PoolManager pool;
    public Button unitSpawnRangeButton;

    public Button reRoll;

    private void Awake()
    {
        unit = GetComponent<Unit>();

        GameObject go = GameObject.Find("PoolManager");
        pool = go.GetComponent<PoolManager>();

        unitSpawnRangeButton = BattleManager.Instance.unitSpawnRange.GetComponentInChildren<Button>();

        reRoll = BattleManager.Instance.reRoll;
    }

    private void Update()
    {
        // 터치 입력을 감지하여 유닛을 생성하는 코드
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = touch.position;
                // 스크린 좌표를 월드 좌표로 변환
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f)); // z는 카메라 거리로 조정
                // 유닛을 생성할 위치 지정
                SpawnUnitAtPosition(worldPosition);
            }
        }

        // PC 키보드 입력 (필요시)
        if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKeyDown(KeyCode.A))
        {
            UiManager.Instance.cost += 30;
        }
    }

    // 유닛을 지정된 위치에 스폰하는 메서드
    private void SpawnUnitAtPosition(Vector3 position)
    {
        // 유닛을 풀에서 가져와서 지정된 위치에 스폰하는 로직
        // 예시로, 유닛 스폰 위치와 풀 관리 방식은 상황에 맞게 수정
        pool.Get(0, 0);  // 실제 유닛을 풀에서 가져오는 방법을 추가
        unit.transform.position = position;
    }

    public void UsingCard()
    {
        // 기존 카드 사용 로직
        if (!BattleManager.Instance.unitSpawnRange.activeSelf)
        {
            foreach (GameObject card in BattleManager.Instance.cardObj)
            {
                if (card != this.gameObject)
                {
                    card.GetComponent<Button>().enabled = false;
                }
            }
            reRoll.enabled = false;

            GameObject spawnArea = BattleManager.Instance.unitSpawnRange.transform.GetChild(1).gameObject;
            RectTransform spawnAreaAnchors = spawnArea.GetComponent<RectTransform>();

            if (BattleManager.Instance.mainCamera.position.x >= 3)
            {
                BattleManager.Instance.mainCamera.position = new Vector3(0, 0, -10);
                spawnAreaAnchors.anchorMin = new Vector2(0.15f, 0.43f);
                spawnAreaAnchors.anchorMax = new Vector2(1, 0.66f);
            }
            else
            {
                BattleManager.Instance.mainCamera.position = new Vector3(0, 0, -10);
                spawnAreaAnchors.anchorMin = new Vector2(0.15f, 0.43f);
                spawnAreaAnchors.anchorMax = new Vector2(1, 0.66f);
            }
            BattleManager.Instance.unitSpawnRange.SetActive(true);
            unitSpawnRangeButton.onClick.RemoveAllListeners();
            unitSpawnRangeButton.onClick.AddListener(() => Buy(unit.cost));
            SummonUnit.instance.ClearCursor(true);
            SummonUnit.instance.GetSkeletonData(unit);
        }
        else
        {
            foreach (GameObject card in BattleManager.Instance.cardObj)
            {
                if (card != this.gameObject)
                {
                    card.GetComponent<Button>().enabled = true;
                }
            }
            reRoll.enabled = true;
            BattleManager.Instance.unitSpawnRange.SetActive(false);
            SummonUnit.instance.GetSkeletonData(null);
            SummonUnit.instance.ClearCursor(false);
        }
    }

    public void UnitSpawn(int unitID)
    {
        BattleManager.Instance.point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -Camera.main.transform.position.z));

        // 유닛 생성 로직...
    }

    public void Buy(int unitCost)
    {
        if (UiManager.Instance != null && UiManager.Instance.cost >= unitCost)
        {
            UiManager.Instance.cost -= unitCost;
            UnitSpawn(unit.unitID);
            SummonUnit.instance.GetSkeletonData(null);
            SummonUnit.instance.ClearCursor(false);
        }
        else
        {
            Debug.Log("코스트가 부족합니다. ");
        }
    }
}
