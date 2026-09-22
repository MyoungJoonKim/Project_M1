using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    [Header("Option Settings")]
    public bool isJoystickVisible = true;
    public bool isDamageTextVisible = true;
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;

    [Header("User Data")]
    public UserData userData = new UserData();

    [Header("Ability Data")]
    [SerializeField] private List<AbilityData> abilitiyDatas = new List<AbilityData>();


    //private void Start()
    //{
    //    PlayerPrefs.DeleteAll();
    //    PlayerPrefs.Save();
    //}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadUserData();
            LoadOptionData();
        }
        else
        Destroy(gameObject);
        
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void SaveUserData()
    {
        PlayerPrefs.SetString("UserName", userData.userName);
        PlayerPrefs.SetInt("UserLevel", userData.userLevel);
        PlayerPrefs.SetInt("UserGold", userData.gold);
        PlayerPrefs.SetFloat("UserExp", userData.userExp);
        PlayerPrefs.SetFloat("UserMaxExp", userData.userMaxExp);

        PlayerPrefs.Save();
    }

    public void SaveOptionData()
    {
        PlayerPrefs.SetInt("JoystickVisible", isJoystickVisible ? 1 : 0);
        PlayerPrefs.SetInt("DamageTextVisible", isDamageTextVisible ? 1 : 0);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.Save();
    }

    public void LoadUserData()
    {
        userData.userName = PlayerPrefs.GetString("UserName", userData.userName);
        userData.userLevel = PlayerPrefs.GetInt("UserLevel", userData.userLevel);
        userData.gold = PlayerPrefs.GetInt("UserGold", userData.gold);
        userData.userExp = PlayerPrefs.GetFloat("UserExp", userData.userExp);
        userData.userMaxExp = PlayerPrefs.GetFloat("UserMaxExp", userData.userMaxExp);
    }

    public void LoadOptionData()
    {
        isJoystickVisible = PlayerPrefs.GetInt("JoystickVisible", 1) == 1;
        isDamageTextVisible = PlayerPrefs.GetInt("DamageTextVisible", 1) == 1;
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", masterVolume);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", bgmVolume);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);
    }

    public void SetJoystickVisible(bool visible)
    {
        isJoystickVisible = visible;
        SaveOptionData();
    }

    public void SetDamageTextVisible(bool visible)
    {
        isDamageTextVisible = visible;
        SaveOptionData();
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        SaveOptionData();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        SaveOptionData();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        SaveOptionData();
    }

    public void AddGold(int amount)
    {
        userData.AddGold(amount);
        SaveUserData();
    }

    public void AddUserExp(float amount)
    {
        userData.AddUserExp(amount);
        SaveUserData();
    }

    public int GetUserLevel()
    {
        return userData.userLevel;
    }

    public float GetUserExp()
    {
        return userData.userExp;
    }

    public float GetUserMaxExp()
    {
        return userData.userMaxExp;
    }

    public int GetGold()
    {
        return userData.gold;
    }


    public bool IsAbilityBought(PassiveType type, int level)
    {
        string key = $"Ability_{type}_{level}";

        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    public bool TryBuyAbility(AbilityData data, int level)
    {
        if (data == null)
            return false;

        if (level < 00 ||
            level >= data.unlockLevel.Length ||
            level >= data.unlockCost.Length ||
            level >= data.bonusValue.Length)
            return false;

        PassiveType type = data.abilityType;

        // 해제한 능력 확인
        if (IsAbilityBought(type, level))
            return false;

        // 유저 레벨 확인
        if (GetUserLevel() < data.unlockLevel[level])
            return false;

        int cost = data.unlockCost[level];

        // 골드 보유 확인
        if (cost < 0 || GetGold() < cost)
            return false;

        userData.AddGold(-cost);

        string key = $"Ability_{type}_{level}";

        PlayerPrefs.SetInt(key, 1);

        SaveUserData();
        return true;
    }

    public float GetAbilityLevel(PassiveType type)
    {
        AbilityData data = GetAbilityData(type);

        if (data == null) 
            return 0;

        float bonus = 0f;

        for (int i = 0; i < data.bonusValue.Length; i++)
        {
            if (IsAbilityBought(type, i))
                bonus = data.bonusValue[i];
        }

        return bonus;
    }

    private AbilityData GetAbilityData(PassiveType type)
    {
        foreach (AbilityData data in abilitiyDatas)
        {
            if (data == null)
                continue;

            if (data.abilityType == type)
                return data;
        }
        return null;
    }
}
