using UnityEngine;
using UnityEngine.EventSystems;

// Battle Canvas 오브젝트에 추가
// Canvas 자체가 드래그 이벤트에 반응해서 움직이는 것을 차단
public class CanvasDragBlocker : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 이벤트 소비 - 아무것도 안 함
        Debug.Log("[CanvasDragBlocker] Canvas 드래그 차단됨");
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 이벤트 소비 - 아무것도 안 함
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 이벤트 소비 - 아무것도 안 함
    }
}