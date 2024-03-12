using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("GameSpeed")]
    private int time = 1;
    [SerializeField] private TextMeshProUGUI gameSpeed;
    public void SpeedUp()
    {
        if (time == 3)
        {
            time = 1;
            Time.timeScale = 1;
            gameSpeed.text = "1X";
        }
        else
        {
            time++;
            if (time == 2) { Time.timeScale = 2; gameSpeed.text = "2X"; }
            else if (time == 3) { Time.timeScale = 3; gameSpeed.text = "3X"; }
        }
    }
}
