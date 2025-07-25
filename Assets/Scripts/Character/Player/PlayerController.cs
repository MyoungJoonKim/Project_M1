using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject weapon;


    public PlayerAnimator PlayerAnimator;
    private Rigidbody rigidbody;
    private Vector3 dir  = Vector3.zero;
    private bool battleState = false;
    private bool ground;
    private bool attacking = false;
    public float inputMagnitude;
    public float speed = 5f;
    public float rotationSpeed = 15f;
    public float jumpHeight = 4f;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<PlayerAnimator>();
        weapon.gameObject.SetActive(false);
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
        inputMagnitude = dir.magnitude;
        this.gameObject.transform.position += dir * speed * Time.deltaTime; // 이동
        
        if (dir != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 8f;
                inputMagnitude = 1.5f;
            }
            else
            {
                speed = 5f;
            }
        }
        EquipWeapon();

        if (battleState)
        {
            PlayerAnimator.animator.SetBool("OnSwordShield_Idle", true);
            PlayerAnimator.SetMove(-1f);
            PlayerAnimator.SetBattleState(inputMagnitude);
        }
        else if (attacking)
        {

        }
        else 
        {
            PlayerAnimator.animator.SetBool("OnSwordShield_Idle", false);
            PlayerAnimator.SetBattleState(-1f);
            PlayerAnimator.SetMove(inputMagnitude);
        }
        


        //CheckGround();
        //if (Input.GetButtonDown("Jump") && ground)
        //{
        //    Vector3 jumpPower = Vector3.up * jumpHeight;
        //    rigidbody.AddForce(jumpPower, ForceMode.VelocityChange);
        //}
    }
    
    void EquipWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            battleState = !battleState;
            if (battleState)
            { 
                weapon.gameObject.SetActive(true); 
            }
            else weapon.gameObject.SetActive(false);
        }
    }

    public void OnButtonAttack()
    {
        if (battleState)
        {
            attacking = true;
            PlayerAnimator.SetAttack(1f);
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
