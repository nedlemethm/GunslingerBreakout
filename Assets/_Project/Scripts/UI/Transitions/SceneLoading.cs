using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] private bool loadsPlayerLevel;
    [SerializeField] private string mainScene;
    [SerializeField] private string uiScene;
    [SerializeField] private string playerScene;

    public void LoadScene()
    {
        if (loadsPlayerLevel)
        {
            StartCoroutine(LoadLevel());
        }
        else
        {
            SceneManager.LoadScene(mainScene);
        }
    }

    private IEnumerator LoadLevel()
    {
        Scene lastScene = SceneManager.GetActiveScene();

        yield return StartCoroutine(LoadSceneAdd(playerScene));
        yield return StartCoroutine(LoadSceneAdd(uiScene));
        yield return StartCoroutine(LoadSceneAdd(mainScene));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainScene));

        SceneManager.UnloadSceneAsync(lastScene);
    }

    private IEnumerator LoadSceneAdd(string sceneName)
    {
        AsyncOperation sceneAsync = new();
        sceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        sceneAsync.allowSceneActivation = false;

        while (sceneAsync.progress < 0.9f)
        {
            yield return null;
        }

        sceneAsync.allowSceneActivation = true;

        while (!sceneAsync.isDone)
        {
            yield return null;
        }
    }
}
