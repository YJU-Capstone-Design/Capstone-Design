using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioBox : MonoBehaviour
{
    public static AudioBox instance;

    public GameObject audioBox;

    public Slider master_Sound;
    public Slider bg_Sound;
    public Slider effect_Sound;
    

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OpenAudioBox(){audioBox.SetActive(true);}
    public void CloseAudioBox(){   audioBox.SetActive(true);}


}
