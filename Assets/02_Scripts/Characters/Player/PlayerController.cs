using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Move Controller")]
    [SerializeField] private JoyStick JoyStick;

    private PlayerAnimator playerAnimator;
    private Player player;
    private Rigidbody2D rb;
    private float speed = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (player == null || player.isDead)
        {
            rb.velocity = Vector2.zero;
            playerAnimator.SetMove(false);
            return;
        }

        // 플레이어 조이스틱 이동
        Vector2 input = JoyStick.Input;

        if (input == null)
            return;

        Vector2 move = input.normalized;
        rb.velocity = move * speed;


        // 플레이어 좌우 반전
        float moveX = rb.velocity.x;
        Vector3 scale = transform.localScale;

        if (moveX > 0) 
            scale.x = Mathf.Abs(scale.x);
        else if (moveX < 0)
            scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;


        // 플레이어 이동 애니메이션
        if (rb.velocity != Vector2.zero)
            playerAnimator.SetMove(true);
        else
            playerAnimator.SetMove(false);
    }
}
