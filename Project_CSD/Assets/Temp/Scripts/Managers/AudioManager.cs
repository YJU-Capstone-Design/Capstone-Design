using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    //배경음
    [SerializeField] AudioSource audioSource_Bg;
    [SerializeField] AudioClip[] audioClip_Bg;
    //버튼음
    [SerializeField] AudioSource audioSource_Btn;
    [SerializeField] AudioClip[] audioClip_Btn;
    //효과음
    [SerializeField] AudioSource audioSource_Effect;
    [SerializeField] AudioClip[] audioClip_Effect;

    public static AudioManager instance;

    public GameObject audioBox;
    public Slider master_Sound;
    public Slider bgm_slider;
    public Slider sfx_slider;


    [Header("버튼")]
    public bool lobbyscene=true;
    public GameObject soundBtn;
    public GameObject endBtn;

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
        // 초기 볼륨 설정
        ChangeBgmSound(25);
        ChangeSfxSound(25);
        if (soundBtn == null)
        {
            soundBtn = GameObject.Find("SoundBtn");
            Button btn = soundBtn.GetComponent<Button>();
            btn.onClick.AddListener(OpenAudioBox);
        }
        if (endBtn == null)
        {
            endBtn = GameObject.Find("EndBtn");
            Button btn = endBtn.GetComponent<Button>();
            btn.onClick.AddListener(EndGame);
        }

    }
    public void GachaSound() {audioSource_Bg.clip = audioClip_Bg[1]; audioSource_Bg.Play();}//가챠룸 사운드
    public void BattleSound() { audioSource_Bg.clip = audioClip_Bg[2]; audioSource_Bg.Play(); }

    public void MainSound() { audioSource_Bg.clip = audioClip_Bg[0]; audioSource_Bg.Play(); }
    public void ButtonSound()
    {
        audioSource_Btn.clip = audioClip_Btn[1];
        audioSource_Btn.Play();
        Debug.Log("ButtonSound");
    }

    public void GachaEffect(bool open)//가챠 사운드
    {
        if (open)//오븐 알람소리
        {
            audioSource_Effect.clip = audioClip_Effect[4];
            audioSource_Effect.Play();
        }
        else//오븐땡소리
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
    public void OpenAudioBox() {
        
        audioBox.SetActive(true);  ButtonSound(); Debug.Log("사운드 설정");
    }
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

    public void BtnClear()
    {
        if (lobbyscene)
        {
            endBtn = GameObject.Find("EndBtn");
            Button btn = endBtn.GetComponent<Button>();
            btn.onClick.AddListener(EndGame);
            Debug.Log("종료 버튼 찾음");
        }
        if (lobbyscene)
        {
            soundBtn = GameObject.Find("SoundBtn");
            Button btn = soundBtn.GetComponent<Button>();
            btn.onClick.AddListener(OpenAudioBox);
            Debug.Log("오디오 버튼 찾음");
        }
    }

    public void EndGame()
    {
        #if UNITY_EDITOR // 유니티 에디터에서 실행 중일 때
            EditorApplication.isPlaying = false;
            Debug.Log("디버그 모드에서 게임 종료");
        #else // 빌드된 파일에서 실행 중일 때
            Application.Quit();
            Debug.Log("빌드된 파일에서 게임 종료");
        #endif
    }
}
