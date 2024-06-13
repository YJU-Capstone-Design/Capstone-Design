using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] Vector2 center;    // 0, 0
    [SerializeField] Vector2 mapSize;   // 배경 너비, 높이
    [SerializeField] Transform tf_cursor;
    [SerializeField] float dragSpeed = 10.0f;   // 화면 움직임 속도

    private float camWidth , camHeight;  // 카메라 너비/2, 높이/2
    private float firstClickPointX;
    private RectTransform tf_background;    // 배경 너비, 높이를 가져오기 위한 변수
    void Start()
    {
        camHeight = Camera.main.orthographicSize;   // 카메라의 높이 / 2
        camWidth = camHeight * Screen.width / Screen.height;    // 카메라의 너비 / 2

        tf_background = GameObject.Find("Background").GetComponent<RectTransform>();

        mapSize.x = tf_background.rect.width;   // 배경의 너비
        mapSize.y = tf_background.rect.height;  // 배경의 높이
    }
    void Update()
    {
        if (BattleManager.Instance.unitSpawnRange.activeSelf == false) //unitSpawnRange가 활성화되면 아래 함수들이 작동을 멈춘다 게이야
        {
            ViewMoving();
            CursorMoving();
        }

    }

    void CursorMoving()
    {
        // 마우스 이동
        float x = Input.mousePosition.x - (Screen.width / 2);
        float y = Input.mousePosition.y - (Screen.height / 2);
        tf_cursor.localPosition = new Vector2(x, y);

        // 마우스 가두기 (범위 지정)
        float tmp_cursorPosX = tf_cursor.localPosition.x;
        float tmp_cursorPosY = tf_cursor.localPosition.y;

        float min_width = -Screen.width / 2;
        float max_width = Screen.width / 2;
        float min_height = -Screen.height / 2;
        float max_height = Screen.height / 2;
        int padding = 20;   // 값은 자유

        tmp_cursorPosX = Mathf.Clamp(tmp_cursorPosX, min_width + padding, max_width - padding);
        tmp_cursorPosY = Mathf.Clamp(tmp_cursorPosY, min_height + padding, max_height - padding);

        tf_cursor.localPosition = new Vector2(tmp_cursorPosX, tmp_cursorPosY);
    }

    void ViewMoving()
    {
        // 마우스 최초 클릭 시의 위치 기억
        if (Input.GetMouseButtonDown(0))
        {
            firstClickPointX = tf_cursor.localPosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            if (Camera.main.transform.position.x >= 0)
            {
                // (현재 마우스 위치 - 최초 위치)의 음의 방향으로 카메라 이동
                Vector2 position = Camera.main.ScreenToViewportPoint(-new Vector3(tf_cursor.localPosition.x - firstClickPointX, 0, 0));
                Vector2 move = position * (Time.deltaTime * dragSpeed);

                Camera.main.transform.Translate(move);

                float dx = mapSize.x;
                float clampX = Mathf.Clamp(Camera.main.transform.position.x, -dx + center.x, dx + center.x);

                //float dy = mapSize.y - camHeight;
                //float clampY = Mathf.Clamp(Camera.main.transform.position.y, -dy + center.y, dy + center.y);

                Camera.main.transform.position = new Vector3(clampX, 0, Camera.main.transform.position.z);
            }
            else if (Camera.main.transform.position.x < 0)
            {
                Camera.main.transform.position = new Vector3(0, 0, -10);
            }
            if (Camera.main.transform.position.x > 20)
            {
                Camera.main.transform.position = new Vector3(20, 0, -10);
            }
        }
    }
}