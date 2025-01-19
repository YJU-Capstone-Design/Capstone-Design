using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor.Experimental.GraphView;  // 이 부분을 삭제하거나 변경합니다.
using UnityEngine.UIElements;  // UIElements를 여기에서 사용


public class CursorController : MonoBehaviour
{
    [SerializeField] Vector2 center;    // 0, 0
    [SerializeField] Vector2 mapSize;   // ��� �ʺ�, ����
    [SerializeField] Transform tf_cursor;
    [SerializeField] float dragSpeed = 10.0f;   // ȭ�� ������ �ӵ�

    private float camWidth , camHeight;  // ī�޶� �ʺ�/2, ����/2
    private float firstClickPointX;
    private RectTransform tf_background;    // ��� �ʺ�, ���̸� �������� ���� ����

    public bool modeCheck = true;
    void Start()
    {
        camHeight = Camera.main.orthographicSize;   // ī�޶��� ���� / 2
        camWidth = camHeight * Screen.width / Screen.height;    // ī�޶��� �ʺ� / 2

        tf_background = GameObject.Find("Background").GetComponent<RectTransform>();

        mapSize.x = tf_background.rect.width;   // ����� �ʺ�
        mapSize.y = tf_background.rect.height;  // ����� ����
    }
    void Update()
    {
        if (BattleManager.Instance.unitSpawnRange.activeSelf == false) //unitSpawnRange�� Ȱ��ȭ�Ǹ� �Ʒ� �Լ����� �۵��� ����� ���̾�
        {
            ViewMoving();
            CursorMoving();
        }

    }

    void CursorMoving()
    {
        if (modeCheck)
        {
            // ���콺 �̵�
            float x = Input.mousePosition.x - (Screen.width / 2);
            float y = Input.mousePosition.y - (Screen.height / 2);
            tf_cursor.localPosition = new Vector2(x, y);

            // ���콺 ���α� (���� ����)
            float tmp_cursorPosX = tf_cursor.localPosition.x;
            float tmp_cursorPosY = tf_cursor.localPosition.y;

            float min_width = -Screen.width / 2;
            float max_width = Screen.width / 2;
            float min_height = -Screen.height / 2;
            float max_height = Screen.height / 2;
            int padding = 20;   // ���� ����

            tmp_cursorPosX = Mathf.Clamp(tmp_cursorPosX, min_width + padding, max_width - padding);
            tmp_cursorPosY = Mathf.Clamp(tmp_cursorPosY, min_height + padding, max_height - padding);

            tf_cursor.localPosition = new Vector2(tmp_cursorPosX, tmp_cursorPosY);
        }
       
       else{
           
                // ���콺 �̵� (PC������ ���콺, ����Ͽ����� ù ��° ��ġ�� ��ġ�� ���)
                float x = 0, y = 0;
                if (Input.touchCount > 0)
                {
                    x = Input.GetTouch(0).position.x - (Screen.width / 2);
                    y = Input.GetTouch(0).position.y - (Screen.height / 2);
                }
                else if (Input.mousePresent)
                {
                    x = Input.mousePosition.x - (Screen.width / 2);
                    y = Input.mousePosition.y - (Screen.height / 2);
                }

                tf_cursor.localPosition = new Vector2(x, y);

                // ���콺/��ġ ���α� (���� ����)
                float tmp_cursorPosX = tf_cursor.localPosition.x;
                float tmp_cursorPosY = tf_cursor.localPosition.y;

                float min_width = -Screen.width / 2;
                float max_width = Screen.width / 2;
                float min_height = -Screen.height / 2;
                float max_height = Screen.height / 2;
                int padding = 20; // ���� ����

                tmp_cursorPosX = Mathf.Clamp(tmp_cursorPosX, min_width + padding, max_width - padding);
                tmp_cursorPosY = Mathf.Clamp(tmp_cursorPosY, min_height + padding, max_height - padding);

                tf_cursor.localPosition = new Vector2(tmp_cursorPosX, tmp_cursorPosY);
            

        }
    }

    void ViewMoving()
    {
        if (modeCheck)
        {
            // ���콺 ���� Ŭ�� ���� ��ġ ���
            if (Input.GetMouseButtonDown(0))
            {
                firstClickPointX = tf_cursor.localPosition.x;
            }

            if (Input.GetMouseButton(0))
            {
                if (Camera.main.transform.position.x >= 0)
                {
                    // (���� ���콺 ��ġ - ���� ��ġ)�� ���� �������� ī�޶� �̵�
                    Vector2 position = Camera.main.ScreenToViewportPoint(-new Vector3(tf_cursor.localPosition.x - firstClickPointX, 0, 0));
                    Vector2 move = position * (Time.deltaTime * dragSpeed);

                    Camera.main.transform.Translate(move);

                    float dx = mapSize.x;
                    float clampX = Mathf.Clamp(Camera.main.transform.position.x, -dx + center.x, dx + center.x);

                    //float dy = mapSize.y - camHeight;
                    //float clampY = Mathf.Clamp(Camera.main.transform.position.y, -dy + center.y, dy + center.y);

                    Camera.main.transform.position = new Vector3(clampX, 0, Camera.main.transform.position.z);
                }
                if (Camera.main.transform.position.x < 0)
                {
                    Camera.main.transform.position = new Vector3(0, 0, -10);
                }
                if (Camera.main.transform.position.x > 20)
                {
                    Camera.main.transform.position = new Vector3(20, 0, -10);
                }
            }
        }
        else
        {
            // ��ġ ���� ���� ��ġ ���
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstClickPointX = tf_cursor.localPosition.x;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // ù ��° ��ġ�� ��ġ ��ȭ�� ���� ī�޶� �̵�
                float touchDeltaX = Input.GetTouch(0).deltaPosition.x;
                Vector2 position = Camera.main.ScreenToViewportPoint(-new Vector3(touchDeltaX, 0, 0));
                Vector2 move = position * (Time.deltaTime * dragSpeed);

                Camera.main.transform.Translate(move);

                float dx = mapSize.x;
                float clampX = Mathf.Clamp(Camera.main.transform.position.x, -dx + center.x, dx + center.x);

                Camera.main.transform.position = new Vector3(clampX, 0, Camera.main.transform.position.z);
            }
        }

        


    }
}