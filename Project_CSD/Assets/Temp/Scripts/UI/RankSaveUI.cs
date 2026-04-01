using UnityEngine;
using TMPro;
using System;

public class RankSaveUI : MonoBehaviour
{
    [SerializeField] GameObject savePanel;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TextMeshProUGUI scoreText;

    private int currentScore;

    
    public void OnClickRank()
    {
        savePanel.SetActive(true);
    }

    public void OnClickSave()
    {
        string playerName = string.IsNullOrEmpty(nameInput.text) ? "이름없음" : nameInput.text;
        if (int.TryParse(scoreText.text, out int parsedScore))
            currentScore = parsedScore;
        else
            currentScore = 0;
        RankingManager.Instance.AddRanking(playerName, currentScore);
        savePanel.SetActive(false);
        Debug.Log("저장 데이터 -> "+ "이름 : "+playerName +" 점수 : "+ currentScore);
    }

    public void OnClickCancel()
    {
        savePanel.SetActive(false);
    }
}