using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private UnitData unitData;

    Unit unit;
    private PoolManager pool;
    public Button unitSpawnRangeButton;

<<<<<<< HEAD
=======
    public Button reRoll;

>>>>>>> origin/Card
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
                    reRoll.enabled = false;
                }
            }

<<<<<<< HEAD
        /*        // 메인 카메라의 위치에 따라 스폰 가능 영역 범위 변경
                if (BattleManager.Instance.mainCamera.position.x >= 3)
                {
                    BattleManager.Instance.mainCamera.position = new Vector3(0,0,-10);
                    spawnAreaAnchors.anchorMin = new Vector2(0, 0.43f);
                    spawnAreaAnchors.anchorMax = new Vector2(1, 0.66f);
                } 좌표세이브 주석처리 06/12일 카드 클릭시 소한 범위 조정을 위한 카메라 이동
                else
                {
                    spawnAreaAnchors.anchorMin = new Vector2(0.15f, 0.43f);
                    spawnAreaAnchors.anchorMax = new Vector2(1, 0.66f);
                }*/

        // 메인 카메라의 위치에 따라 스폰 가능 영역 범위 변경
        if (BattleManager.Instance.mainCamera.position.x >= 3)
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
=======
            GameObject spawnArea = BattleManager.Instance.unitSpawnRange.transform.GetChild(1).gameObject;
            RectTransform spawnAreaAnchors = spawnArea.GetComponent<RectTransform>();

            // 메인 카메라의 위치에 따라 스폰 가능 영역 범위 변경
            if (BattleManager.Instance.mainCamera.position.x >= 3)
            {
                spawnAreaAnchors.anchorMin = new Vector2(0, 0.43f);
                spawnAreaAnchors.anchorMax = new Vector2(1, 0.66f);
            }
            else
            {
                spawnAreaAnchors.anchorMin = new Vector2(0.15f, 0.43f);
                spawnAreaAnchors.anchorMax = new Vector2(1, 0.66f);
            }
            BattleManager.Instance.unitSpawnRange.SetActive(true);
            unitSpawnRangeButton.onClick.RemoveAllListeners();
            Debug.Log(unit.unitID);
            /*        unitSpawnRangeButton.onClick.AddListener(() => UnitSpawn(unit.unitID));*/
            unitSpawnRangeButton.onClick.AddListener(() => Buy(unit.cost));
        } else
        {
            // 해당 카드의 사용을 캔슬할 경우 다른 카드의 버튼 컴포넌트 활성화
            foreach (GameObject card in BattleManager.Instance.cardObj)
            {
                if (card != this.gameObject)
                {
                    card.GetComponent<Button>().enabled = true;
                    reRoll.enabled = true;
                }
            }
            BattleManager.Instance.unitSpawnRange.SetActive(false);
        }
>>>>>>> origin/Card
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
            case 11004: // Croirang
                pool.Get(0, 5);
                break;
        }

        BattleManager.Instance.unitSpawnRange.SetActive(false);
        BattleManager.Instance.CardShuffle(false);
    }


    public void Buy(int unitCost)
    {
        // uiMgr.cost와 unitData.Cost를 비교하여 구매 가능한지 확인
        if (UiManager.Instance != null && UiManager.Instance.cost >= unitCost)
        {
            // 코스트를 차감하고 유닛을 스폰
            UiManager.Instance.cost -= unitCost;
            UnitSpawn(unit.unitID); //52줄에 있는 데이터 담기
        }
        else
        {
            Debug.Log("돈이 없다! 게이게이야! ");
        }
    }
}