using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTitle : MonoBehaviour
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
        Shared.TextBlink.textBlink = false;
        gamePlayButton.gameObject.SetActive(false);
        loadingBar.gameObject.SetActive(true);
        StartCoroutine(LoadingBarUpdate(loadingBar, 2f));
    }

    private IEnumerator LoadingBarUpdate(Slider bar, float timer)
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
        Shared.sceneLoadManager.ChangeScene(SceneType.LOBBY, false);
    }
}
