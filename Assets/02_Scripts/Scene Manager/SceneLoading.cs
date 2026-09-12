using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class SceneLoading : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text gameTipText;


    private List<string> gameTipKeys = new List<string>()
    {
        "TIP_001",
        "TIP_002",
        "TIP_003",
        "TIP_004",
        "TIP_005",
    };


    private void Start()
    {
        StartCoroutine(LoadBattleScene());
    }

    private IEnumerator LoadBattleScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync($"{SceneLoadManager.Instance.nextScene}");

        asyncOperation.allowSceneActivation = false;    

        if (loadingBar == null)
            yield break;

        loadingBar.minValue = 0f;
        loadingBar.maxValue = 1f;
        loadingBar.value = 0f;

        StartCoroutine(BackgroundTextUpdate());

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            loadingBar.value = progress;

            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(3f);

                loadingBar.maxValue = 1f;
                
                StopCoroutine(BackgroundTextUpdate());

                asyncOperation.allowSceneActivation = true;
                SoundManager.Instance.PlaySceneBGM(SceneLoadManager.Instance.nextScene);
            }
            yield return null;
        }
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
        int rand = Random.Range(0, gameTipKeys.Count);

        gameTipText.text = LocalizationSettings.StringDatabase.GetLocalizedString("GameTip_Text", gameTipKeys[rand]);
    }
}
