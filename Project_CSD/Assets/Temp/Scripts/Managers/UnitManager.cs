using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// UnitCardDragBlocker 삭제 후 인터페이스 직접 구현으로 복구
public class UnitManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private UnitData unitData;

    Unit unit;
    private PoolManager pool;
    public Button reRoll;

    public static bool isDraggingAny = false;

    private GameObject dragPreview;
    private bool isDragging = false;
    private bool isInSpawnArea = false;

    private RectTransform spawnAreaRect;
    private Camera uiCamera;

    // ─── 프리뷰 전용 오버레이 캔버스 (static: 씬에 하나만) ───
    private static Canvas _overlayCanvas;

    private static Canvas GetOrCreateOverlayCanvas()
    {
        if (_overlayCanvas != null) return _overlayCanvas;

        GameObject go = new GameObject("DragOverlayCanvas");
        Canvas c = go.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        c.sortingOrder = 9999;
        go.AddComponent<CanvasScaler>();
        go.AddComponent<GraphicRaycaster>();
        _overlayCanvas = c;
        return _overlayCanvas;
    }

    private void Awake()
    {
        unit = GetComponent<Unit>();

        GameObject go = GameObject.Find("PoolManager");
        pool = go.GetComponent<PoolManager>();

        reRoll = BattleManager.Instance.reRoll;

        GameObject uiCamObj = GameObject.Find("UI Canvas Camera");
        if (uiCamObj != null)
            uiCamera = uiCamObj.GetComponent<Camera>();

        GetOrCreateOverlayCanvas();
    }

    // ─────────────────────────────────────────────
    // 카드 터치 시작
    // ─────────────────────────────────────────────
    public void OnPointerDown(PointerEventData eventData)
    {
        eventData.Use();
        if (UiManager.Instance == null || UiManager.Instance.cost < unit.cost)
        {
            Debug.Log("잔액 부족!");
            return;
        }

        isDraggingAny = true;
        isDragging = true;
        isInSpawnArea = false;

        BattleManager.Instance.unitSpawnRange.SetActive(true);

        Transform spawnAreaTf = BattleManager.Instance.unitSpawnRange.transform.GetChild(1);
        if (spawnAreaTf == null)
        {
            Debug.LogError("unitSpawnRange의 자식(1)이 없습니다.");
            isDragging = false;
            isDraggingAny = false;
            BattleManager.Instance.unitSpawnRange.SetActive(false);
            return;
        }
        spawnAreaRect = spawnAreaTf.GetComponent<RectTransform>();

        SetCardsInteractable(false);
        //CreateDragPreview(eventData.position);

        SummonUnit.instance.ClearCursor(true);
        SummonUnit.instance.GetSkeletonData(unit);
    }

    // ─────────────────────────────────────────────
    // 드래그 중
    // ─────────────────────────────────────────────
    public void OnDrag(PointerEventData eventData)
    {
        eventData.Use();

        if (!isDragging) return;

        if (dragPreview != null)
            dragPreview.GetComponent<RectTransform>().position = eventData.position;

        MoveSummonUnitToScreenPos(eventData.position);

        isInSpawnArea = spawnAreaRect != null &&
            RectTransformUtility.RectangleContainsScreenPoint(
                spawnAreaRect, eventData.position, uiCamera);

        if (dragPreview != null)
        {
            Image img = dragPreview.GetComponent<Image>();
            if (img != null)
                img.color = isInSpawnArea
                    ? new Color(1f, 1f, 1f, 0.8f)
                    : new Color(1f, 0.3f, 0.3f, 0.6f);
        }
    }

    // ─────────────────────────────────────────────
    // 손 뗌
    // ─────────────────────────────────────────────
    public void OnPointerUp(PointerEventData eventData)
    {
        eventData.Use();
        isDraggingAny = false;

        if (!isDragging)
        {
            SetCardsInteractable(true);
            BattleManager.Instance.unitSpawnRange.SetActive(false);
            return;
        }

        isDragging = false;
        //DestroyDragPreview();

        if (isInSpawnArea)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(eventData.position.x, eventData.position.y,
                    -Camera.main.transform.position.z));
            worldPos.z = 0f;
            BattleManager.Instance.point = worldPos;

            UiManager.Instance.cost -= unit.cost;
            UnitSpawn(unit.unitID);

            SummonUnit.instance.ClearCursor(false);
            SummonUnit.instance.GetSkeletonData(null);
        }
        else
        {
            Debug.Log("소환 취소 (범위 밖)");
            CancelSpawn();
        }
    }

    // ─────────────────────────────────────────────
    // 실제 유닛 소환
    // ─────────────────────────────────────────────
    private void UnitSpawn(int unitID)
    {
        int index = GetPoolIndex(unitID);
        if (index >= 0)
            pool.GetAtPoint(0, index);
        else
            Debug.LogWarning($"알 수 없는 unitID: {unitID}");

        SetCardsInteractable(true);
        BattleManager.Instance.unitSpawnRange.SetActive(false);
        BattleManager.Instance.CardShuffle(false);
    }

    // ─────────────────────────────────────────────
    // 소환 취소
    // ─────────────────────────────────────────────
    private void CancelSpawn()
    {
        SetCardsInteractable(true);
        BattleManager.Instance.unitSpawnRange.SetActive(false);
        SummonUnit.instance.GetSkeletonData(null);
        SummonUnit.instance.ClearCursor(false);
    }

    // ─────────────────────────────────────────────
    // 드래그 프리뷰 생성 / 제거
    // ─────────────────────────────────────────────
    private void CreateDragPreview(Vector2 screenPos)
    {
        Canvas overlay = GetOrCreateOverlayCanvas();

        dragPreview = new GameObject("DragPreview");
        dragPreview.transform.SetParent(overlay.transform, false);
        dragPreview.transform.SetAsLastSibling();

        Image img = dragPreview.AddComponent<Image>();
        img.sprite = unit.data.Unit_CardImg;
        img.color = new Color(1f, 1f, 1f, 0.8f);
        img.raycastTarget = false;

        RectTransform rt = dragPreview.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(100, 140);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.position = screenPos;
    }

    private void DestroyDragPreview()
    {
        if (dragPreview != null)
        {
            Destroy(dragPreview);
            dragPreview = null;
        }
    }

    // ─────────────────────────────────────────────
    // 스켈레톤 프리뷰 월드 좌표 이동
    // ─────────────────────────────────────────────
    private void MoveSummonUnitToScreenPos(Vector2 screenPos)
    {
        if (SummonUnit.instance == null) return;

        Camera cam = Camera.main;
        Vector3 worldPos = cam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, -cam.transform.position.z));
        worldPos.z = 0f;

        SummonUnit.instance.transform.position = worldPos;
    }

    // ─────────────────────────────────────────────
    // 카드 & 리롤 인터랙션 일괄 제어
    // ─────────────────────────────────────────────
    private void SetCardsInteractable(bool interactable)
    {
        foreach (GameObject card in BattleManager.Instance.cardObj)
        {
            if (card == null) continue;
            if (!interactable && card == this.gameObject) continue;

            Button btn = card.GetComponent<Button>();
            if (btn != null) btn.enabled = interactable;
        }

        if (reRoll != null) reRoll.enabled = interactable;
    }

    // ─────────────────────────────────────────────
    // unitID → PoolManager 인덱스 변환
    // ─────────────────────────────────────────────
    private int GetPoolIndex(int unitID)
    {
        switch (unitID)
        {
            case 11001: return 0;
            case 11002: return 1;
            case 11003: return 2;
            case 12001: return 3;
            case 12002: return 4;
            case 12003: return 7;
            case 11004: return 5;
            case 11005: return 6;
            case 11006: return 8;
            case 11007: return 9;
            default: return -1;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKeyDown(KeyCode.A))
            UiManager.Instance.cost += 30;
    }
#endif
}
