using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UserData
{
    public Sprite userIcon;
    public string userName;
    public int userLevel = 1;
    public float userExp = 0f;
    public float userMaxExp = 100f;

    public int gold = 0;

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public void AddUserExp(float amount)
    {
        userExp += amount;

        while (userExp >= userMaxExp)
        {
            userExp -= userMaxExp;
            UserLevelUp();
        }
    }

    private void UserLevelUp()
    {
        userLevel++;
        userMaxExp *= 1.5f;
    }
}
