using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CashManager : MonoBehaviour
{
    public CashManager instance;
    [Header("Money")]
    public int player_Gold;
    public int player_Cash;
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI CashText;
    public GameObject obj1;
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

    public void Update()
    {
        if(obj1 == null)
        {
            obj1 = GameObject.Find("Cash (TMP)");
            Debug.Log(obj1.gameObject.ToString());
            
        }
        
        UpGold();

    }
    public void UpGold()
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
                    Debug.LogError("TextMeshProUGUI ������Ʈ�� ã�� �� �����ϴ�.");
                }
            }
        }

    }

    public void PlusGold()
    {
        player_Gold += 1;
    }

}
