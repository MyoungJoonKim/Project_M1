using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SceneLoading : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text gameTipText;


    private readonly List<string> gameTipKeys = new List<string>()
    {
        "TIP_001",
        "TIP_002",
        "TIP_003",
        "TIP_004",
        "TIP_005",
    };

    private readonly List<string> loadedGameTips = new List<string>();

    private Coroutine backgroundTextCoroutine;

    private void Start()
    {
        StartCoroutine(LoadBattleScene());
    }

    private IEnumerator LoadBattleScene()
    {
        if (SceneLoadManager.Instance == null)
            yield break;
        
        SceneType nextScene = SceneLoadManager.Instance.nextScene;

        if (loadingBar != null)
        {
            loadingBar.minValue = 0f;
            loadingBar.maxValue = 1f;
            loadingBar.value = 0f;
        }

        yield return null;

        yield return LocalizationSettings.InitializationOperation;

        loadedGameTips.Clear();

        if (gameTipText != null)
        {
            foreach (string key in gameTipKeys)
            {
                var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("GameTip_Text", key);

                yield return operation;

                if (operation.Status == AsyncOperationStatus.Succeeded && !string.IsNullOrEmpty(operation.Result))
                {
                    loadedGameTips.Add(operation.Result);
                }
            }
        }

        backgroundTextCoroutine = StartCoroutine(BackgroundTextUpdate());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextScene.ToString());

        if (asyncOperation == null)
        {
            Debug.LogError("SceneLoading: 씬 로딩 요청 실패");

            if (backgroundTextCoroutine != null)
            {
                StopCoroutine(backgroundTextCoroutine);
                backgroundTextCoroutine = null;
            }
            yield break;
        }

        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            if (loadingBar != null)
            {
                loadingBar.value =
                    Mathf.Clamp01(asyncOperation.progress / 0.9f);
            }
            yield return null;
        }

        if (loadingBar != null)
            loadingBar.value = 1f;

        yield return new WaitForSecondsRealtime(3f);

        if (backgroundTextCoroutine != null)
        {
            StopCoroutine(backgroundTextCoroutine);
            backgroundTextCoroutine = null;
        }

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySceneBGM(nextScene);

        asyncOperation.allowSceneActivation = true;
    }

    private IEnumerator BackgroundTextUpdate()
    {
        while (true)
        {
            GameTipText();
            yield return new WaitForSeconds(2f);
        }
    }

    private void GameTipText()
    {
        if (gameTipText == null || loadedGameTips.Count == 0)
            return;

        int rand = Random.Range(0, loadedGameTips.Count);

        gameTipText.text = loadedGameTips[rand];
    }
}
