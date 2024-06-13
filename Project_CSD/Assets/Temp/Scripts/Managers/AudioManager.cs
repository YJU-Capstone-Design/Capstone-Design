using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    //�����
    [SerializeField] AudioSource audioSource_Bg;
    [SerializeField] AudioClip[] audioClip_Bg;
    //��ư��
    [SerializeField] AudioSource audioSource_Btn;
    [SerializeField] AudioClip[] audioClip_Btn;
    //ȿ����
    [SerializeField] AudioSource audioSource_Effect;
    [SerializeField] AudioClip[] audioClip_Effect;

    private void Start()
    {
       
        audioSource_Bg.clip = audioClip_Bg[0];
        audioSource_Bg.Play();
    }




    public void ButtonSound()
    {
        audioSource_Btn.clip = audioClip_Btn[1];
        audioSource_Btn.Play();
        Debug.Log("ButtonSound");
    }

}
