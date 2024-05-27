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
        while (true)
        {
            yield return null;
            Debug.Log(nextScene);
            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;
            float timer = 0.0f;
            while (!op.isDone)
            {
                yield return null;
                timer += Time.unscaledDeltaTime;
                if (op.progress < 0.9f)
                {
                    progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer*speed);
                    if (progressBar.value >= op.progress)
                    {
                        timer = 0f;
                    }
                }
                else
                {
                    progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer*speed);
                    if (progressBar.value == 1f)
                    {
                        // �ε��� �Ϸ�� �Ŀ� nextScene ������ �ʱ�ȭ�մϴ�.
                        nextScene = "";
                        op.allowSceneActivation = true;
                        break; // �ε��� �Ϸ�Ǿ����Ƿ� �̹� �� �ε� �ڷ�ƾ�� �����մϴ�.
                    }
                    time.text = (progressBar.value * 100).ToString("F0") + "%";
                }
            }
        }
    }
}
