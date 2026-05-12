using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Title : MonoBehaviour
{
    [Header("Title Loading Bar")]
    public Slider loadingBar;

    [Header("Title GamePlay Button")]
    public GameObject gamePlayButton;

    private void Start()
    {
        if (loadingBar != null)
            loadingBar.gameObject.SetActive(false);
    }
    public void OnButtonGamePlay()
    {
        Shared.TMP_TextBlink.textBlink = false;
        gamePlayButton.gameObject.SetActive(false);
        loadingBar.gameObject.SetActive(true);
        StartCoroutine(LoadingBarUpdate(loadingBar, 5f));
    }

    IEnumerator LoadingBarUpdate(Slider bar, float timer)
    {
        if (bar == null)
            yield break;

        bar.value = bar.minValue;
        bar.maxValue = timer;

        while (true)
        {
            bar.value += 0.02f;
            yield return new WaitForSeconds(0.01f);
            if (bar.value == timer)
                break;
        }
        Shared.scene_Manager.ChangeScene(SceneType.LOBBY, false);
    }
}
