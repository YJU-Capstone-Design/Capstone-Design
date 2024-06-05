using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CashManager : MonoBehaviour
{
    public CashManager instance;
    [Header("Money")]
    public int player_Cash;
    public int player_Gold;
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI CashText;
    public GameObject obj1;
    public GameObject obj2;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        var obj = FindObjectsOfType<CashManager>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        UpGold();   
    }

    public void CheakMoney()
    {
        if (obj1 == null)
        {
            obj1 = GameObject.Find("Gold (TMP)");
            Debug.Log(obj1.gameObject.ToString());
        }
        if (obj2 == null)
        {
            obj2 = GameObject.Find("Cash (TMP)");
            Debug.Log(obj2.gameObject.ToString());
        }

        UpGold();
    }

    public void UpGold() //CHAT GPT 사용함
    {
        if (obj1 != null)
        {
            // TextMeshPro 컴포넌트를 가져옵니다.
            TextMeshProUGUI tmp = obj1.GetComponent<TextMeshProUGUI>();
            string a = player_Gold.ToString();
            if (!tmp.Equals(a))
            {
                // 텍스트 값을 변경합니다.
                if (tmp != null)
                {
                    tmp.text = player_Gold.ToString();
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI Game의 컴포넌트를 찾을 수 없습니다.");
                }
            }
        }

        if (obj2 != null)
        {
            TextMeshProUGUI tmp2 = obj2.GetComponent<TextMeshProUGUI>();
            string a = player_Cash.ToString();
            if (!tmp2.Equals(a))
            {
                if (tmp2 != null)
                {
                    tmp2.text = player_Cash.ToString();
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI Cash의 컴포넌트를 찾을 수 없습니다.");
                }
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 로드된 씬의 이름이 "TargetScene"일 경우 함수를 실행
        if (scene.name == "MainLobby")
        {
            CheakMoney();
        }
    }
}
