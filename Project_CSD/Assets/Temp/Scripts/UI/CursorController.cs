using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] Vector2 center;
    [SerializeField] Vector2 mapSize;
    [SerializeField] Transform tf_cursor;
    [SerializeField] float dragSpeed = 10.0f;

    private float camWidth, camHeight;
    private float firstClickPointX;
    private RectTransform tf_background;

    void Start()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Screen.width / Screen.height;
        tf_background = GameObject.Find("Background").GetComponent<RectTransform>();
        mapSize.x = tf_background.rect.width;
        mapSize.y = tf_background.rect.height;

        // 모바일 실기기에서는 커서 UI 숨김
#if UNITY_ANDROID || UNITY_IOS
        tf_cursor.gameObject.SetActive(false);
#else
        if (Input.touchSupported)
            tf_cursor.gameObject.SetActive(false);
#endif
    }

    void Update()
    {
        if (BattleManager.Instance.unitSpawnRange.activeSelf == false)
        {
#if UNITY_ANDROID || UNITY_IOS
            // 빌드된 모바일에서는 무조건 터치
            if (Input.touchCount > 0)
                TouchViewMoving();
#else
            // PC / 에디터 시뮬레이터
            if (Input.touchCount > 0)
            {
                TouchViewMoving();
            }
            else
            {
                Vector3 mPos = Input.mousePosition;
                if (!float.IsInfinity(mPos.x) && !float.IsInfinity(mPos.y)
                    && !float.IsNaN(mPos.x) && !float.IsNaN(mPos.y)
                    && Screen.width > 0 && Screen.height > 0)
                {
                    CursorMoving();
                    ViewMoving();
                }
            }
#endif
        }
    }

    // ────────────────────────────────────────────
    // PC: 마우스 커서 이동
    // ────────────────────────────────────────────
    void CursorMoving()
    {
        Vector3 mPos = Input.mousePosition;

        float x = mPos.x - (Screen.width * 0.5f);
        float y = mPos.y - (Screen.height * 0.5f);

        if (float.IsInfinity(x) || float.IsInfinity(y) ||
            float.IsNaN(x) || float.IsNaN(y))
            return;

        tf_cursor.localPosition = new Vector2(x, y);

        float tmp_cursorPosX = tf_cursor.localPosition.x;
        float tmp_cursorPosY = tf_cursor.localPosition.y;
        float min_width = -Screen.width * 0.5f;
        float max_width = Screen.width * 0.5f;
        float min_height = -Screen.height * 0.5f;
        float max_height = Screen.height * 0.5f;
        int padding = 20;

        tmp_cursorPosX = Mathf.Clamp(tmp_cursorPosX, min_width + padding, max_width - padding);
        tmp_cursorPosY = Mathf.Clamp(tmp_cursorPosY, min_height + padding, max_height - padding);
        tf_cursor.localPosition = new Vector2(tmp_cursorPosX, tmp_cursorPosY);
    }

    // ────────────────────────────────────────────
    // PC: 마우스 드래그 카메라 이동
    // ────────────────────────────────────────────
    void ViewMoving()
    {
        if (Input.GetMouseButtonDown(0))
            firstClickPointX = tf_cursor.localPosition.x;

        if (Input.GetMouseButton(0))
        {
            if (Camera.main.transform.position.x >= 0)
            {
                Vector2 position = Camera.main.ScreenToViewportPoint(
                    -new Vector3(tf_cursor.localPosition.x - firstClickPointX, 0, 0));
                Vector2 move = position * (Time.deltaTime * dragSpeed);
                Camera.main.transform.Translate(move);

                float dx = mapSize.x;
                float clampX = Mathf.Clamp(Camera.main.transform.position.x,
                                           -dx + center.x, dx + center.x);
                Camera.main.transform.position =
                    new Vector3(clampX, 0, Camera.main.transform.position.z);
            }

            if (Camera.main.transform.position.x < 0)
                Camera.main.transform.position = new Vector3(0, 0, -10);

            if (Camera.main.transform.position.x > 20)
                Camera.main.transform.position = new Vector3(20, 0, -10);
        }
    }

    // ────────────────────────────────────────────
    // 모바일: 터치 드래그 카메라 이동
    // ────────────────────────────────────────────
    void TouchViewMoving()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
            firstClickPointX = touch.position.x;

        if (touch.phase == TouchPhase.Moved)
        {
            float delta = touch.position.x - firstClickPointX;
            firstClickPointX = touch.position.x;

            float move = -(delta / Screen.width) * dragSpeed;
            Camera.main.transform.Translate(move, 0, 0); // Time.deltaTime 제거

            float clampX = Mathf.Clamp(Camera.main.transform.position.x, 0, 20);
            Camera.main.transform.position = new Vector3(clampX, 0, -10);
        }
    }
}