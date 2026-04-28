using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDrop_Manager : MonoBehaviour
{
    [Header("Exp Gem Prefab")]
    public ExpGem expGemPrefab;


    private void Awake()
    {
        if (Shared.expDrop_Manager == null)
        {
            Shared.expDrop_Manager = this;
        }
    }

    public void SpawnExpGem(Vector3 position, float expAmount)
    {
        ExpGem gem = Instantiate(expGemPrefab, position, Quaternion.identity);
        gem.Init(expAmount);
    }

    

    
}
