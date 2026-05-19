using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDrop_Manager : MonoBehaviour
{
    [Header("Exp Gem Prefab")]
    public ExpGem[] expGemPrefab;


    private void Awake()
    {
        if (Shared.expDrop_Manager == null)
        {
            Shared.expDrop_Manager = this;
        }
    }

    public void SpawnExpGem(Vector3 position, float expAmount)
    {
        ExpGem prefab = GetExpGemPrefab(expAmount);

        if (prefab == null)
            return;

        ExpGem gem = Instantiate(prefab, position, Quaternion.identity);
        gem.Init(expAmount);
    }

    private ExpGem GetExpGemPrefab(float expAmount)
    {
        int index = Mathf.FloorToInt((expAmount - 1) / 50f);

        if (index < 0)
            index = 0;

        if (index >= expGemPrefab.Length)
            index = expGemPrefab.Length - 1;

        return expGemPrefab[index];
    }

    

    
}
