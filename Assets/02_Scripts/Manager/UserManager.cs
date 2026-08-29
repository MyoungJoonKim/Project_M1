using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

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
