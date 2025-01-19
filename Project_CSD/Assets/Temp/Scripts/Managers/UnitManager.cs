using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] public UnitData unitData;
    private Unit unit;
    private PoolManager pool;

    public Button unitSpawnRangeButton;
    public Button reRoll;

    private Camera mainCamera;
    private Vector3 defaultCameraPosition;

    private void Awake()
    {
        // 초기화
        unit = GetComponent<Unit>();
        pool = GameObject.Find("PoolManager").GetComponent<PoolManager>();
        unitSpawnRangeButton = BattleManager.Instance.unitSpawnRange.GetComponentInChildren<Button>();
        reRoll = BattleManager.Instance.reRoll;

        mainCamera = Camera.main;
        defaultCameraPosition = new Vector3(0, 0, -10);

        // 초기화 확인
        if (unit == null)
        {
            Debug.LogError("Unit component not found on this GameObject!");
        }

        if (pool == null)
        {
            Debug.LogError("PoolManager is not found!");
        }
    }

    private void Update()
    {
        HandleTouchInput();
        HandleDebugInput();
    }

    // 터치 입력을 처리하는 메서드
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                SpawnUnitAtPosition(worldPosition);
            }
        }
    }

    // 디버그용 입력 처리 (PC에서의 키보드 입력)
    private void HandleDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKeyDown(KeyCode.A))
        {
            UiManager.Instance.cost += 30;
        }
    }

    // 유닛을 지정된 위치에 스폰하는 메서드
    private void SpawnUnitAtPosition(Vector3 position)
    {
        // pool에서 유닛을 가져옵니다 (GameObject 반환)
        GameObject unitObject = pool.Get(0, 0);  // 실제 유닛을 풀에서 가져오는 방법
        if (unitObject != null)
        {
            // GameObject에서 Unit 컴포넌트를 가져옵니다
            Unit newUnit = unitObject.GetComponent<Unit>();
            if (newUnit != null)
            {
                newUnit.transform.position = position;  // 유닛의 위치 설정
            }
            else
            {
                Debug.LogError("Unit component not found on the GameObject!");
            }
        }
        else
        {
            Debug.LogError("Failed to get GameObject from pool!");
        }
    }


    // 카드를 사용할 때 호출되는 메서드
    public void UsingCard()
    {
        bool isSpawnRangeActive = BattleManager.Instance.unitSpawnRange.activeSelf;

        if (!isSpawnRangeActive)
        {
            DisableOtherCards();
            reRoll.enabled = false;

            UpdateSpawnArea();

            BattleManager.Instance.unitSpawnRange.SetActive(true);
            unitSpawnRangeButton.onClick.RemoveAllListeners();
            unitSpawnRangeButton.onClick.AddListener(() => Buy(unit.cost));

            SummonUnit.instance.ClearCursor(true);
            SummonUnit.instance.GetSkeletonData(unit);
        }
        else
        {
            EnableOtherCards();
            reRoll.enabled = true;

            BattleManager.Instance.unitSpawnRange.SetActive(false);
            SummonUnit.instance.GetSkeletonData(null);
            SummonUnit.instance.ClearCursor(false);
        }
    }

    // 다른 카드들을 비활성화하는 메서드
    private void DisableOtherCards()
    {
        foreach (GameObject card in BattleManager.Instance.cardObj)
        {
            if (card != gameObject)
            {
                card.GetComponent<Button>().enabled = false;
            }
        }
    }

    // 다른 카드들을 활성화하는 메서드
    private void EnableOtherCards()
    {
        foreach (GameObject card in BattleManager.Instance.cardObj)
        {
            if (card != gameObject)
            {
                card.GetComponent<Button>().enabled = true;
            }
        }
    }

    // 유닛 소환 범위 영역을 업데이트하는 메서드
    private void UpdateSpawnArea()
    {
        GameObject spawnArea = BattleManager.Instance.unitSpawnRange.transform.GetChild(1).gameObject;
        RectTransform spawnAreaAnchors = spawnArea.GetComponent<RectTransform>();

        mainCamera.transform.position = defaultCameraPosition;  // 카메라 위치 수정
        spawnAreaAnchors.anchorMin = new Vector2(0.15f, 0.43f);
        spawnAreaAnchors.anchorMax = new Vector2(1, 0.66f);
    }

    // 유닛을 실제로 스폰하는 메서드
    public void UnitSpawn(int unitID)
    {
        Vector3 spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        // 유닛 생성 로직...
        // 예시로, 유닛을 생성하는 풀을 사용할 수 있습니다.
    }

    // 유닛을 구매하는 메서드
    public void Buy(int unitCost)
    {
        if (UiManager.Instance != null && UiManager.Instance.cost >= unitCost)
        {
            UiManager.Instance.cost -= unitCost;

            // 유닛 카드 클릭 시 unitData를 사용하여 정확한 유닛 인덱스 전달
            int unitIndex = unitData.unitID; // unitData의 unitID를 사용하여 유닛 종류 지정

            // 유닛 소환
            GameObject unitObject = pool.Get(0, unitIndex);  // 0은 유닛 프리팹 배열, unitIndex는 유닛 종류
            unitObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            // 유닛 스폰 후 다른 작업 수행
            SummonUnit.instance.GetSkeletonData(null);
            SummonUnit.instance.ClearCursor(false);
        }
        else
        {
            Debug.Log("코스트가 부족합니다.");
        }
    }

}
