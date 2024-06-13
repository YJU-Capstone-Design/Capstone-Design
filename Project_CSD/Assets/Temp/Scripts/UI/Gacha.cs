using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gacha : MonoBehaviour
{
    

    [SerializeField] GameObject screen;
    [SerializeField] Image obj_Img;
    [SerializeField] Sprite basic_Img;
    [SerializeField] private UnitData[] unit_DB;
    [SerializeField] private SpellData[] spell_DB;
    [SerializeField] GameObject holding;
    [SerializeField] RectTransform animTr;
    public float duration = 2.0f; // 애니메이션이 완료될 때까지 걸리는 시간 (초)
    public void Init(UnitData data)
    {
        obj_Img.sprite = data.Unit_Img;
        gameObject.SetActive(true);
       
    }
    public void SpellInit(SpellData data)
    {
        obj_Img.sprite = data.Spell_CardImg;
        gameObject.SetActive(true);
       
    }
    private Coroutine anchorAnimationCoroutine;
    private void OnEnable()
    {
        // 게임 오브젝트가 활성화될 때 코루틴 시작
        AnimTr();
    }

    private void AnimTr()
    {
        // 진행 중인 코루틴이 있으면 중지
        if (anchorAnimationCoroutine != null)
        {
            StopCoroutine(anchorAnimationCoroutine);
        }

        // 새로운 코루틴 시작
        anchorAnimationCoroutine = StartCoroutine(AnimateAnchorMinToMaxY(animTr, animTr.anchorMin.y, animTr.anchorMax.y, duration));
    }

    private IEnumerator AnimateAnchorMinToMaxY(RectTransform target, float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 경과된 시간의 비율 계산
            float t = elapsedTime / duration;

            // Lerp를 사용하여 값 보간
            float newY = Mathf.Lerp(startValue, endValue, t);

            // Y축 앵커 min 값을 새로운 값으로 설정
            target.anchorMin = new Vector2(target.anchorMin.x, newY);

            // 경과된 시간 증가
            elapsedTime += Time.deltaTime;

            // 한 프레임 대기
            yield return null;
        }

        // 애니메이션이 완료된 후 최종 값 설정
        target.anchorMin = new Vector2(target.anchorMin.x, endValue);
        anchorAnimationCoroutine = null; // 코루틴이 종료되었으므로 null로 설정
        yield break;
    }
}
