using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loading;
    [SerializeField] private Slider loadingSlider;
    
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        while (!op.isDone)
        {
            loading.SetActive(true);

            float progress = Mathf.Clamp01(op.progress / .9f);
            loadingSlider.value = progress;

            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
