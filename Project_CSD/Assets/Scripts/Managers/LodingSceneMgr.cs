using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LodingSceneMgr : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressBar;
    public float loadingSpeed = 0.2f;
    [SerializeField] TextMeshProUGUI time;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loding");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    // 로딩이 완료된 후에 nextScene 변수를 초기화합니다.
                    nextScene = "";
                    op.allowSceneActivation = true;
                    yield break;
                }
                time.text = (progressBar.fillAmount * 100).ToString("F0") + "%";
                Debug.Log("Loading Progress: " + (progressBar.fillAmount * 100).ToString("F0") + "%");
            }
        }
    }
}
