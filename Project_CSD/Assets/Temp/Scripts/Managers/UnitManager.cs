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
    public void UsingCard()
    {
        if (!BattleManager.Instance.unitSpawnRange.activeSelf)
        {
            // 해당 카드를 제외한 카드의 버튼 컴포넌트를 비활성화 처리
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

            // 메인 카메라의 위치에 따라 스폰 가능 영역 범위 변경
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
            Debug.Log(unit.unitID);
            /*        unitSpawnRangeButton.onClick.AddListener(() => UnitSpawn(unit.unitID));*/
            unitSpawnRangeButton.onClick.AddListener(() => Buy(unit.cost));
        }
        else
        {
            // 해당 카드의 사용을 캔슬할 경우 다른 카드의 버튼 컴포넌트 활성화
            foreach (GameObject card in BattleManager.Instance.cardObj)
            {
                if (card != this.gameObject)
                {
                    card.GetComponent<Button>().enabled = true;
                }
            }
            reRoll.enabled = true;
            BattleManager.Instance.unitSpawnRange.SetActive(false);
        }
    }

    public void UnitSpawn(int unitID)
    {
        // 마우스 좌클릭 한 곳의 위치값
        BattleManager.Instance.point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -Camera.main.transform.position.z));

        switch (unitID)
        {
            case 11001: // Kitchu
                pool.Get(0, 0);
                break;
            case 11002: // Ramo
                pool.Get(0, 1);
                break;
            case 11003: // Pupnut
                pool.Get(0, 2);
                break;
            case 12001: // WhiteBread
                pool.Get(0, 3);
                break;
            case 12002: // BreadCrab
                pool.Get(0, 4);
                break;
            case 12003: // PanCake
                pool.Get(0, 7);
                break;
            case 11004: // Croirang
                pool.Get(0, 5);
                break;
            case 11005: // Eggball
                pool.Get(0, 6);
                break;
            case 11006: //Turtle
                pool.Get(0, 8);
                break;
            case 11007: // Froll
                pool.Get(0, 9);
                break;
        }

        foreach (GameObject card in BattleManager.Instance.cardObj)
        {
            card.GetComponent<Button>().enabled = true;
        }
        reRoll.enabled = true;

        BattleManager.Instance.unitSpawnRange.SetActive(false);
        BattleManager.Instance.CardShuffle(false);

        // usingCount 테이블에 해당 ID 의 컬럼에 count 값에 +1, 해당 ID 컬럼이 없으면 먼저 추가
        XmlNodeList cardData = DBConnect.Select("usingCount", $"WHERE cardID = {unitID}");

        if (cardData != null)
        {
            DBConnect.UpdateOriginal($"UPDATE usingCount SET count = count + 1 WHERE cardID = {unitID}");
        }
        else
        {
            Debug.Log("입력되어있는 카드값이 없습니다. 그러니 새로 추가 합니다.");
            // 새로 컬럼 추가
            DBConnect.Insert("usingCount", $"{unitID}, 1");
        }
    }


    public void Buy(int unitCost)
    {
        // uiMgr.cost와 unitData.Cost를 비교하여 구매 가능한지 확인
        if (UiManager.Instance != null && UiManager.Instance.cost >= unitCost)
        {
            // 코스트를 차감하고 유닛을 스폰
            UiManager.Instance.cost -= unitCost;
            UnitSpawn(unit.unitID);
        }
        else
        {
            Debug.Log("돈이 없다! 게이게이야! ");
        }
    }
}