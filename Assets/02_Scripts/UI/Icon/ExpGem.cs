using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    private ExpDropManager manager;
    private int poolIndex;
    private float expAmount;
    public void SetManager(ExpDropManager manager)
    {
        this.manager = manager;
    }

    public void SetPoolIndex(int index)
    {
        poolIndex = index;
    }

    public int GetPoolIndex()
    {
        return poolIndex;
    }

    public void Init(float expAmount)
    {
        this.expAmount = expAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player == null)
            return;

        player.AddExp(expAmount);

        if (manager != null)
            manager.Release(this);
        else
            gameObject.SetActive(false);
    }
}
