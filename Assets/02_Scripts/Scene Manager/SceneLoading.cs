using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public Slider loadingBar;
    //public Text Texts;
    //public Image Background;
    //public Sprite[] Images;

    //List<string> texts = new List<string>();


    void Start()
    {
        StartCoroutine(LoadingBarUpdate(loadingBar,5f));
        //StartCoroutine(BackgroundUpdate(5f));
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
        SceneManager.LoadScene((int)Shared.sceneLoadManager.nextScene);
    }


    //IEnumerator BackgroundUpdate(float timer)
    //{
    //    while (true)
    //    {
    //        RandomUpdateBackground();
    //        RandomUpdateText();
    //        yield return new WaitForSeconds(2f);

    //        if (LoadingBar.value == timer)
    //            break;
    //    }
    //}

    //public void RandomUpdateBackground()
    //{
    //    int rand = Random.Range(0, Images.Length);
    //    Background.sprite = Images[rand];
    //}

    //public void RandomUpdateText()
    //{
    //    texts.Add("1.");
    //    texts.Add("2,");
    //    texts.Add("3.");
    //    texts.Add("4.");
    //    texts.Add("5.");

    //    int rand = Random.Range(0, texts.Count);
    //    Texts.text = texts[rand];
    //}
}
