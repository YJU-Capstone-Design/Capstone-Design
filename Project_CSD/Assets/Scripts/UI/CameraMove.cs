using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
/*    [SerializeField] float dragSpeed = 10.0f;   // ȭ�� ������ �ӵ�
    private float firstClickPointX;*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
/*    void Update()
    {
        // ...
        ViewMoving();
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
        }
    }*/
}
