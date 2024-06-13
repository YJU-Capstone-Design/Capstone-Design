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
    public float duration = 2.0f; // �ִϸ��̼��� �Ϸ�� ������ �ɸ��� �ð� (��)
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
        // ���� ������Ʈ�� Ȱ��ȭ�� �� �ڷ�ƾ ����
        AnimTr();
    }

    private void AnimTr()
    {
        // ���� ���� �ڷ�ƾ�� ������ ����
        if (anchorAnimationCoroutine != null)
        {
            StopCoroutine(anchorAnimationCoroutine);
        }

        // ���ο� �ڷ�ƾ ����
        anchorAnimationCoroutine = StartCoroutine(AnimateAnchorMinToMaxY(animTr, animTr.anchorMin.y, animTr.anchorMax.y, duration));
    }

    private IEnumerator AnimateAnchorMinToMaxY(RectTransform target, float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // ����� �ð��� ���� ���
            float t = elapsedTime / duration;

            // Lerp�� ����Ͽ� �� ����
            float newY = Mathf.Lerp(startValue, endValue, t);

            // Y�� ��Ŀ min ���� ���ο� ������ ����
            target.anchorMin = new Vector2(target.anchorMin.x, newY);

            // ����� �ð� ����
            elapsedTime += Time.deltaTime;

            // �� ������ ���
            yield return null;
        }

        // �ִϸ��̼��� �Ϸ�� �� ���� �� ����
        target.anchorMin = new Vector2(target.anchorMin.x, endValue);
        anchorAnimationCoroutine = null; // �ڷ�ƾ�� ����Ǿ����Ƿ� null�� ����
        yield break;
    }
}
