using UnityEngine;

public class UserManager : MonoBehaviour
{
    public UserData userData = new UserData();

    
    private void Awake()
    {
        if (CoreService.userManager != null && CoreService.userManager != this)
        {
            Destroy(gameObject);
            return;
        }
        CoreService.userManager = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (CoreService.userManager == this)
            CoreService.userManager = null;
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
