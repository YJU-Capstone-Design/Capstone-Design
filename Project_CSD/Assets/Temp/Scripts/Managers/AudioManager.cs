using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
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

    public static AudioManager instance;

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
    private void Start()
    {

        MainSound();
    }

    public void GachaSound() {audioSource_Bg.clip = audioClip_Bg[1]; audioSource_Bg.Play();}
    public void BattleSound() { audioSource_Bg.clip = audioClip_Bg[2]; audioSource_Bg.Play(); }

    public void MainSound() { audioSource_Bg.clip = audioClip_Bg[0]; audioSource_Bg.Play(); }
    public void ButtonSound()
    {
        audioSource_Btn.clip = audioClip_Btn[1];
        audioSource_Btn.Play();
        Debug.Log("ButtonSound");
    }
   
}
