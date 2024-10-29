using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    Vector3 pos;
    public float speed = 1f;
    RectTransform rectTransform;
    void Start()
    {
       /* rectTransform= GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(10, 10);*/
    }

    // Update is called once per frame
    void Update()
    {
        pos = Input.mousePosition;
        pos.z = speed;
        pos.x -= 10;
        pos.y += 10;
        transform.position = Camera.main.ScreenToWorldPoint(pos);

    }
  
}
