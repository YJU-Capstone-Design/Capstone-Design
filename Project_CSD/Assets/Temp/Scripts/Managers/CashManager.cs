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

    public void UpGold() //CHAT GPT �����
    {
        if (obj1 != null)
        {
            // TextMeshPro ������Ʈ�� �����ɴϴ�.
            TextMeshProUGUI tmp = obj1.GetComponent<TextMeshProUGUI>();
            string a = player_Gold.ToString();
            if (!tmp.Equals(a))
            {
                // �ؽ�Ʈ ���� �����մϴ�.
                if (tmp != null)
                {
                    tmp.text = player_Gold.ToString();
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI Game�� ������Ʈ�� ã�� �� �����ϴ�.");
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
                    Debug.LogError("TextMeshProUGUI Cash�� ������Ʈ�� ã�� �� �����ϴ�.");
                }
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �ε�� ���� �̸��� "TargetScene"�� ��� �Լ��� ����
        if (scene.name == "MainLobby")
        {
            CheakMoney();
        }
    }
}
