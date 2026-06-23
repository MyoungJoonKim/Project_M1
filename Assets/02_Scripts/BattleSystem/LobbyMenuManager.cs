using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuManager : MonoBehaviour
{
    [Header("Lobby Menu Toggles")]
    [SerializeField] private Toggle storeMenu;
    [SerializeField] private Toggle equipMenu;
    [SerializeField] private Toggle battleMenu;
    [SerializeField] private Toggle etcMenu;
    [SerializeField] private Toggle passiveMenu;

    public bool storeMenuLock = true;
    public bool equipMenuLock = true;
    public bool battleMenuLock = false;
    public bool etcMenuLock = true;
    public bool passiveMenuLock = true;


    private void Awake()
    {
        Shared.lobbyMenuManager = this;
    }

    private void OnDestroy()
    {
        if (Shared.lobbyMenuManager == this)
            Shared.lobbyMenuManager = null;
    }

    private void MenuLockOff()
    {
        if (Shared.userManager.GetUserLevel() >= 2f)
        {
            passiveMenuLock = false;
        }

        if (Shared.userManager.GetUserLevel() >= 3f)
        {
            equipMenuLock = false;
        }

        if (Shared.userManager.GetUserLevel() >= 5f)
        {
            storeMenuLock = false;
        }
    }
}
