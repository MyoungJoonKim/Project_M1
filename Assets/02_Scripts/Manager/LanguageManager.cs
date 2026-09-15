using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject languagePanel;

    private const string LanguageKey = "Language";

    private bool isChangeLanguage;

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        string saveLanguage = PlayerPrefs.GetString(LanguageKey, "ko");

        SetLocale(saveLanguage);
    }

    public void OnLanguageSetting()
    {
        languagePanel.SetActive(true);
        SoundManager.Instance.PlayButtonClick();
    }

    public void SetKorean()
    {
        ChangeLanguage("ko");
        languagePanel.SetActive(false);
        SoundManager.Instance.PlayButtonClick();
    }

    public void SetEnglish()
    {
        ChangeLanguage("en");
        languagePanel.SetActive(false);
        SoundManager.Instance.PlayButtonClick();
    }

    private void ChangeLanguage(string localeCode)
    {
        if (isChangeLanguage) 
            return;

        StartCoroutine(ChangeLanguageCoroutine(localeCode));
    }

    private IEnumerator ChangeLanguageCoroutine(string localeCode)
    {
        isChangeLanguage = true;

        yield return LocalizationSettings.InitializationOperation;

        SetLocale(localeCode);

        PlayerPrefs.SetString(LanguageKey, localeCode);
        PlayerPrefs.Save();

        isChangeLanguage = false;
    }

    private void SetLocale(string localeCode)
    {
        var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);

        if (locale != null)
            LocalizationSettings.SelectedLocale = locale;
    }
}
