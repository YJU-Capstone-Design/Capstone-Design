using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] Vector2 center;    // 0, 0
    [SerializeField] Vector2 mapSize;   // ��� �ʺ�, ����
    [SerializeField] Transform tf_cursor;
    [SerializeField] float dragSpeed = 10.0f;   // ȭ�� ������ �ӵ�

    private float camWidth, camHeight;  // ī�޶� �ʺ�/2, ����/2
    private float firstClickPointX;
    private RectTransform tf_background;    // ��� �ʺ�, ���̸� �������� ���� ����

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
        ViewMoving();
        CursorMoving();
    }

    void CursorMoving()
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
        int padding = 20;	// ���� ����

        tmp_cursorPosX = Mathf.Clamp(tmp_cursorPosX, min_width + padding, max_width - padding);
        tmp_cursorPosY = Mathf.Clamp(tmp_cursorPosY, min_height + padding, max_height - padding);

        tf_cursor.localPosition = new Vector2(tmp_cursorPosX, tmp_cursorPosY);
    }

    void ViewMoving()
    {
        // ���콺 ���� Ŭ�� ���� ��ġ ���
        if (Input.GetMouseButtonDown(0))
        {
            firstClickPointX = tf_cursor.localPosition.x;
        }

        if (Input.GetMouseButton(0))
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
    }

}
