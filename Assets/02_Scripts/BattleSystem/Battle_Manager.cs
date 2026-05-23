using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Manager : MonoBehaviour
{
    [Header("Character Player")]
    public Player player;


    private void Awake()
    {
        if (Shared.battle_Manager == null)
        {
            Shared.battle_Manager = this;
            DontDestroyOnLoad(this);
        }
    }

    // ∏ÛΩ∫≈Õ ≈∏∞Ÿ √ﬂ¿˚ ∏ÿ√„.
    public void PlayerDead(Player player)
    {
        if (Shared.spawn_Manager != null) 
            Shared.spawn_Manager.ClearMonsterTargets();
    }
}
