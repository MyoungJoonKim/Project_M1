using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerAnimator PlayerAnimator;
    private Rigidbody rigidbody;
    private Vector3 dir  = Vector3.zero;
    private bool ground;

    public float speed = 5f;
    public float rotationSpeed = 15f;
    public float jumpHeight = 4f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<PlayerAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        Rotation();
    }

    void Move()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        dir.Normalize(); // 정규화 
        this.gameObject.transform.position += dir * speed * Time.deltaTime; // 이동

        if (dir != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                PlayerAnimator.StartRunStandard();
            }
            else
                PlayerAnimator.EndRunStandard();
                PlayerAnimator.StartWalkStandard();
        }

        CheckGround();
        if (Input.GetButtonDown("Jump") && ground)
        {
            Vector3 jumpPower = Vector3.up * jumpHeight;
            rigidbody.AddForce(jumpPower, ForceMode.VelocityChange);
        }
    }
    void EndWalkStandard()
    {
        if (dir == Vector3.zero)
        {
            PlayerAnimator.animator.SetBool("Walk_Standard", false);
        }
    }
    void Rotation()
    {
        if (dir != Vector3.zero)
        {
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z)) // 부드러운 방향전환
            {
                transform.Rotate(0, 1, 0);
            }
            transform.forward = Vector3.Lerp(transform.forward, dir , rotationSpeed * Time.deltaTime); // 이동 방향 바라보기
        }
    }

    void CheckGround()
    {
        if (transform.position.y < 1.1)
            ground = true;
        else
            ground = false;
    }
}
