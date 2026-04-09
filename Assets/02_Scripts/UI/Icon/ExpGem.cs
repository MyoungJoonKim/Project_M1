using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [SerializeField] private float expAmount = 1f;

    public void Init(float amount)
    {
        expAmount = amount;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player == null)
            return;

        player.AddExp(expAmount);
        gameObject.SetActive(false);
    }
}
