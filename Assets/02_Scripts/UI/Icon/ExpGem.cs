using UnityEngine;

public class ExpGem : MonoBehaviour
{
    private DropManager dropmanager;

    private int poolIndex;
    private float expAmount;

    public void SetManager(DropManager dropmanager)
    {
        this.dropmanager = dropmanager;
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
        Player player = collision.GetComponentInParent<Player>();

        if (player == null)
            return;

        player.AddExp(expAmount);

        if (dropmanager != null)
            dropmanager.ReleaseExpGem(this);
        else
            gameObject.SetActive(false);
    }
}
