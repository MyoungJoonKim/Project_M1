using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [Header("Loading Bar Slider")]
    [SerializeField] private Slider loadingBar;


    private void Start()
    {
        StartCoroutine(LoadingBarUpdate(loadingBar,2f));
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
        Shared.sceneLoadManager.ChangeScene(Shared.sceneLoadManager.nextScene, false);
    }
}
