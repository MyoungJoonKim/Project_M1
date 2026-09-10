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

    public UserData userData = new UserData();

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        Destroy(gameObject);
        
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void SetJoystickVisible(bool visible)
    {
        isJoystickVisible = visible;
    }

    public void SetDamageTextVisible(bool visible)
    {
        isDamageTextVisible = visible;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }

    public void AddGold(int amount)
    {
        userData.AddGold(amount);
    }

    public void AddUserExp(float amount)
    {
        userData.AddUserExp(amount);
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
}
