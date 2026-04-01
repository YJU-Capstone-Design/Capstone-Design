using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject infomation;

    public void OpenInfo()
    {
        infomation.SetActive(true);
    }
    public void CloseInfo()
    {
        infomation.SetActive(false);
    }
}
