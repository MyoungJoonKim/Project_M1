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
        texts.Add("1.ĳ���� �������� ���ؼ� ����ġ�� �ʿ��մϴ�.");
        texts.Add("2.��ų�� ��Ÿ���� �ֽ��ϴ�.");
        texts.Add("3.���� �⺻ ��ȭ�Դϴ�.");
        texts.Add("4.���͸� ������ �������� ȹ���� �� �ֽ��ϴ�.");
        texts.Add("5.�ɼǿ��� ���带 ������ �� �ֽ��ϴ�.");

        int rand = Random.Range(0, texts.Count);
        Texts.text = texts[rand];
    }


}
