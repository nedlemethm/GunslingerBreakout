using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionMain : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text levelTextHighlight;
    [SerializeField] private TMP_Text levelTextDescription;
    [SerializeField] private Image levelPreviewImage;
    [SerializeField] private GameObject previewCanvasGroup;

    [Header("Scene Loading Things")]
    [SerializeField] private string uiScene;
    [SerializeField] private string playerScene;
    private string mainScene;

    private void Start()
    {
        previewCanvasGroup.SetActive(false);
    }

    public void UpdateCurrentLevel(string levelName, string levelDescription, string levelScene, Sprite levelImage)
    {
        if (!previewCanvasGroup.activeSelf)
            previewCanvasGroup.SetActive(true);

        levelText.text = levelName;
        levelTextHighlight.text = levelName;
        levelTextDescription.text = levelDescription;
        levelPreviewImage.sprite = levelImage;

        mainScene = levelScene;
    }

    // Below is a slightly modified version of the SceneLoading bootstrapper
    // Only did this for the sake of time, but ideally this script should inherit from SceneLoading and override the functions to handle the swapping of scene names

    public void LoadScene()
    {
        if (mainScene != null)
            StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        Scene lastScene = SceneManager.GetActiveScene();

        yield return StartCoroutine(LoadSceneAdd(uiScene));
        yield return StartCoroutine(LoadSceneAdd(playerScene));
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
