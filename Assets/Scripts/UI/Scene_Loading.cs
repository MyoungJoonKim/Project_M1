using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Loading : MonoBehaviour
{
    public Slider LoadingBar;
    public Sprite[] Images;
    public Text[] Texts;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingTime(5f));
    }

    IEnumerator LoadingTime(float timer)
    {
        LoadingBar.value = LoadingBar.minValue;
        LoadingBar.maxValue = timer;

        while (true)
        {
            LoadingBar.value++;
            yield return new WaitForSeconds(1f);

            if (LoadingBar.value == timer)
                break;
        }
        SceneManager.LoadScene((int)Shared.Scene_Manager.NextScene);
    }

    
}
