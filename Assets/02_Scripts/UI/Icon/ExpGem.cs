using System.Collections;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private CircleCollider2D playerCollider;
    [SerializeField] private CircleCollider2D pickupRange;

    private DropManager dropmanager;
    private Coroutine gemMoveCoroutine;

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
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        
        Player player = collision.GetComponentInParent<Player>();

        if (player == null)
        {
            Debug.Log("player null");
            return;
        }

        if (collision.CompareTag("Pickup"))
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (gemMoveCoroutine == null)
                gemMoveCoroutine = StartCoroutine(GemMoveCoroutine(player));
        }

        if (collision.CompareTag("Player"))
        {
            if (gemMoveCoroutine != null)
            {
                StopCoroutine(gemMoveCoroutine);
                gemMoveCoroutine = null;
            }

            player.AddExp(expAmount);

            if (dropmanager != null)
                dropmanager.ReleaseExpGem(this);
            else
                gameObject.SetActive(false);
        }
    }

    private IEnumerator GemMoveCoroutine(Player player)
    {
        float speed = 10f;

        while (true)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;

            transform.position += (Vector3)(dir * speed * Time.deltaTime);

            yield return null;
        }
    }
}
