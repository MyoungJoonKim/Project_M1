using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Manager : MonoBehaviour
{
    [Header("Character Player")]
    public Player player;

    [Header("Monsters")]
    public List<Monster> monsters = new();


    private void Awake()
    {
        if (Shared.battle_Manager == null)
        {
            Shared.battle_Manager = this;
            DontDestroyOnLoad(this);
        }
    }

    public void PlayerDead(Player player)
    {
        foreach (var monster in monsters)
        {
            if (monster == null) 
                continue;
            monster.SetTarget(null);
        }
    }
}
