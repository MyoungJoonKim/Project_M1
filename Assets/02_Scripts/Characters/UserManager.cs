using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserManager : MonoBehaviour
{
    public UserData userData = new UserData();

    
    private void Awake()
    {
        if (Shared.userManager != null && Shared.userManager != this)
        {
            Destroy(gameObject);
            return;
        }
        Shared.userManager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Shared.userManager == this)
            Shared.userManager = null;
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
