using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public GameObject audioBox;
    public Slider master_Sound;
    public Slider bgm_slider;
    public Slider sfx_slider;




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
        bgm_slider = bgm_slider.GetComponent<Slider>();
        sfx_slider = sfx_slider.GetComponent<Slider>();
        

        master_Sound.onValueChanged.AddListener(MaserSound);
        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
        sfx_slider.onValueChanged.AddListener(ChangeSfxSound);
    }
    private void Start()
    {

        MainSound();
    }

    public void GachaSound() {audioSource_Bg.clip = audioClip_Bg[1]; audioSource_Bg.Play();}//��í�� ����
    public void BattleSound() { audioSource_Bg.clip = audioClip_Bg[2]; audioSource_Bg.Play(); }

    public void MainSound() { audioSource_Bg.clip = audioClip_Bg[0]; audioSource_Bg.Play(); }
    public void ButtonSound()
    {
        audioSource_Btn.clip = audioClip_Btn[1];
        audioSource_Btn.Play();
        Debug.Log("ButtonSound");
    }

    public void GachaEffect(bool open)//��í ����
    {
        if (open)//���� �˶��Ҹ�
        {
            audioSource_Effect.clip = audioClip_Effect[4];
            audioSource_Effect.Play();
        }
        else//���춯�Ҹ�
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
    public void OpenAudioBox() { audioBox.SetActive(true);  ButtonSound();  }
    public void CloseAudioBox() { audioBox.SetActive(false);  ButtonSound();  }



    void ChangeBgmSound(float value)
    {
        audioSource_Bg.volume = value;
    }

    void ChangeSfxSound(float value)
    {
        audioSource_Btn.volume = value;
        audioSource_Effect.volume = value;
    }
    void MaserSound(float value)
    {
        audioSource_Bg.volume = value;
        audioSource_Btn.volume = value;
        audioSource_Effect.volume = value;
    }
}
