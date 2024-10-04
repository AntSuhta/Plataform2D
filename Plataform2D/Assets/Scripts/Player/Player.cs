using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    
    [Header("Jump")]
    public float jumpForce;
    public bool isJumping;
    public bool doubleJump;
    
    [Header("Anim")]
    public Animator anim;
   
    [Header("Attack")]
    public Transform point;
    public float radius;
    private bool isAttacking;

    Rigidbody2D rig;
    void Start()

    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();


    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {

        float horizontalMove = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(horizontalMove * moveSpeed, rig.velocity.y);

        if (horizontalMove > 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (horizontalMove < 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        if (horizontalMove == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("Transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
            }
            else if (doubleJump)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * (jumpForce - 3), ForceMode2D.Impulse);
                doubleJump = false;
            }
        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetInteger("Transition", 3);
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius);

            if (hit != null)
            {
                Debug.Log(hit.name);
            }

            StartCoroutine(OnAttack());
        }
        
    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.29f);
        isAttacking = false;
    }


    void OnDrawGizmos()
    {
    
        Gizmos.DrawWireSphere(point.position, radius);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Debug.Log("Estou no chão");
            isJumping = false;
            doubleJump = false;
        }
    }
}
