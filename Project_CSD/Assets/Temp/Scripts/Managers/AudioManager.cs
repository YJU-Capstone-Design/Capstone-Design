using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //¹è°æÀ½
    [SerializeField] AudioSource audioSource_Bg;
    [SerializeField] AudioClip[] audioClip_Bg;
    //¹öÆ°À½
    [SerializeField] AudioSource audioSource_Btn;
    [SerializeField] AudioClip[] audioClip_Btn;
    //È¿°úÀ½
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

    public void GachaSound() {audioSource_Bg.clip = audioClip_Bg[1]; audioSource_Bg.Play();}//°¡Ã­·ë »ç¿îµå
    public void BattleSound() { audioSource_Bg.clip = audioClip_Bg[2]; audioSource_Bg.Play(); }

    public void MainSound() { audioSource_Bg.clip = audioClip_Bg[0]; audioSource_Bg.Play(); }
    public void ButtonSound()
    {
        audioSource_Btn.clip = audioClip_Btn[1];
        audioSource_Btn.Play();
        Debug.Log("ButtonSound");
    }

    public void GachaEffect(bool open)//°¡Ã­ »ç¿îµå
    {
        if (open)//¿Àºì ¾Ë¶÷¼Ò¸®
        {
            audioSource_Effect.clip = audioClip_Effect[4];
            audioSource_Effect.Play();
        }
        else//¿Àºì¶¯¼Ò¸®
        {
            audioSource_Effect.clip = audioClip_Effect[5];
            audioSource_Effect.Play();
        }
        
    }

    public void BattleEndSound(bool win)
    {
        if (win)
        {
            audioSource_Effect.clip = audioClip_Effect[0];
            audioSource_Effect.Play();
        }
        else
        {
            int ran=Random.Range(1,3);
            audioSource_Effect.clip = audioClip_Effect[ran];
            audioSource_Effect.Play();
        }

    }
   
}
