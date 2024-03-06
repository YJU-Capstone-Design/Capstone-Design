using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
/*    [SerializeField] float dragSpeed = 10.0f;   // 화면 움직임 속도
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
        // 마우스 최초 클릭 시의 위치 기억
        if (Input.GetMouseButtonDown(0))
        {
            firstClickPointX = tf_cursor.localPosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            // (현재 마우스 위치 - 최초 위치)의 음의 방향으로 카메라 이동
            Vector2 position = Camera.main.ScreenToViewportPoint(-new Vector3(tf_cursor.localPosition.x - firstClickPointX, 0, 0));
            Vector2 move = position * (Time.deltaTime * dragSpeed);

            Camera.main.transform.Translate(move);
        }
    }*/
}
