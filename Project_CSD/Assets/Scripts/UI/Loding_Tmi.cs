using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Loding_Tmi : MonoBehaviour
{
    [SerializeField] private Sprite[] tmi_Img;

    private void Awake()
    {
        Image tmi = this.GetComponent<Image>();
        int ran = Random.Range(0, tmi_Img.Length);

        tmi.sprite = tmi_Img[ran];
        
    }
    
}
