using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Loading : MonoBehaviour
{
    public Slider LoadingBar;
    public Text Texts;
    public Image Background;
    public Sprite[] Images;

    List<string> texts = new List<string>();


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
            RandomUpdateBackground();
            RandomUpdateText();
            yield return new WaitForSeconds(1f);

            if (LoadingBar.value == timer)
                break;
        }
        SceneManager.LoadScene((int)Shared.Scene_Manager.NextScene);
    }

    public void RandomUpdateBackground()
    {
        int rand = Random.Range(0, Images.Length);
        Background.sprite = Images[rand];
    }

    public void RandomUpdateText()
    {
        texts.Add("1.캐릭터 레벨업을 위해선 경험치가 필요합니다.");
        texts.Add("2.스킬은 쿨타임이 있습니다.");
        texts.Add("3.골드는 기본 재화입니다.");
        texts.Add("4.몬스터를 잡으면 아이템을 획득할 수 있습니다.");
        texts.Add("5.옵션에서 사운드를 조절할 수 있습니다.");

        int rand = Random.Range(0, texts.Count);
        Texts.text = texts[rand];
    }


}
