using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CashManager : MonoBehaviour
{
    [Header("Money")]
    public int player_Gold;
    public int player_Cash;
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI CashText;
    // Start is called before the first frame update
    private void Awake()
    {
        var obj = FindObjectsOfType<CashManager>();


        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GoldText.text = player_Gold.ToString();
        CashText.text = player_Cash.ToString();
    }

    public void Update()
    {
        GameObject obj1 = GameObject.Find("Cash (TMP)");
    }

    public void PlusGold()
    {
        player_Gold += 1;
    }

}
