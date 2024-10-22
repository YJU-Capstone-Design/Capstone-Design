using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LodingSceneMgr : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Slider progressBar;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] private float speed;
    [SerializeField] Sprite[] bg;
    [SerializeField] Image bg_Obj;
    private void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
        
        bg_Obj.sprite = bg[Random.Range(0,bg.Length)];
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loding");
    }

    IEnumerator LoadScene()
    {
        // Load the next scene asynchronously but don't activate it yet
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            // Update the progress bar value slowly
            timer += Time.unscaledDeltaTime * speed;
            float targetProgress = op.progress < 0.9f ? op.progress : 1f;
            progressBar.value = Mathf.MoveTowards(progressBar.value, targetProgress, timer * Time.unscaledDeltaTime);

            // Update the progress text
            time.text = (progressBar.value * 100).ToString("F0") + "%";

            // If the progress bar is full, break out of the loop
            if (progressBar.value >= 1f)
            {
                break;
            }
        }

        // Wait for 3 seconds
        yield return new WaitForSeconds(1.0f);

        // Activate the scene
        op.allowSceneActivation = true;
    }
}
