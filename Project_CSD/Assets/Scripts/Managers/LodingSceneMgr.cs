using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LodingSceneMgr : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressBar;
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
                        // �ε��� �Ϸ�� �Ŀ� nextScene ������ �ʱ�ȭ�մϴ�.
                        nextScene = "";
                        op.allowSceneActivation = true;
                        break; // �ε��� �Ϸ�Ǿ����Ƿ� �̹� �� �ε� �ڷ�ƾ�� �����մϴ�.
                    }
                    time.text = (progressBar.fillAmount * 100).ToString("F0") + "%";
                }
            }
        }
    }
}