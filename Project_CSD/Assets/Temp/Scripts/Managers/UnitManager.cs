using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    Unit unit;
    private PoolManager pool;
    public Button unitSpawnRangeButton;

    private void Awake()
    {
        unit = GetComponent<Unit>();

        GameObject go = GameObject.Find("PoolManager");
        pool = go.GetComponent<PoolManager>();

        unitSpawnRangeButton = BattleManager.Instance.unitSpawnRange.GetComponentInChildren<Button>();
    }

    public void UsingCard()
    {
        GameObject spawnArea = BattleManager.Instance.unitSpawnRange.transform.GetChild(1).gameObject;
        RectTransform spawnAreaAnchors = spawnArea.GetComponent<RectTransform>();

        // 메인 카메라의 위치에 따라 스폰 가능 영역 범위 변경
        if (BattleManager.Instance.mainCamera.position.x >= 3)
        {
            spawnAreaAnchors.anchorMin = new Vector2(0, 0.1f);
            spawnAreaAnchors.anchorMax = new Vector2(1, 0.5f);
        }
        else
        {
            spawnAreaAnchors.anchorMin = new Vector2(0.15f, 0.1f);
            spawnAreaAnchors.anchorMax = new Vector2(1, 0.5f);
        }
        BattleManager.Instance.unitSpawnRange.SetActive(true);
        unitSpawnRangeButton.onClick.RemoveAllListeners();
        Debug.Log(unit.unitID);
        unitSpawnRangeButton.onClick.AddListener(() => UnitSpawn(unit.unitID));
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
        }

        BattleManager.Instance.unitSpawnRange.SetActive(false);
        BattleManager.Instance.CardShuffle();
    }
}