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
                    Debug.LogError("TextMeshProUGUI 컴포넌트를 찾을 수 없습니다.");
                }
            }
        }

    }

    public void PlusGold()
    {
        player_Gold += 1;
    }

}
