using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GoblinWithRaycast : MonoBehaviour
{
    [Header("STATUS")]
    public int damage;
    public int health;
    public float speed;


    [Header("Bools")]
    public bool isFront;
    public bool isAttacking;
    public bool isRight;
    private bool isDead;

    [Header("REFS")]
    public Transform behind;
    public Transform raycastPoint;
    public Animator anim;

    Rigidbody2D rig;
    Vector2 direction;



    [Header("RayCast")]
    public float behindVision;
    public float raycastVision;
    public float stopPosition;



 


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        if (isRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;

        }

    }

    private void FixedUpdate()
    {
        GetPlayer();
        OnMove();
    }
    void GetPlayer()
    {

        RaycastHit2D hit = Physics2D.Raycast(raycastPoint.position, direction, raycastVision);

        if (hit.collider != null && !isDead)
        {
            if (hit.transform.CompareTag("Player"))
            {
                isFront = true;

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= stopPosition)
                {
                        isFront = false;
                        rig.velocity = Vector2.zero;
                    
                        hit.transform.GetComponent<Player>().OnHit(damage);     
                    
                        anim.SetInteger("transition", 2);

                }
            }
        }
        RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, behindVision);

        if (behindHit.collider != null) 
        {

            if (behindHit.transform.CompareTag("Player"))
            {
                isFront = true;
                isRight = !isRight;
            }

        }
    }
    void OnMove()
    {
        if (isFront && !isDead)
        {
            anim.SetInteger("transition", 1);

            if (isRight)
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rig.velocity = new Vector2(speed, rig.velocity.y);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                rig.velocity = new Vector2(-speed, rig.velocity.y);

            }
        }

    }
    public void OnHit(int dmg)
    {
        anim.SetTrigger("takeDamage");
        health -= dmg;

        if (health <= 0)
        {
            isDead = false;
            anim.SetTrigger("death");
            Destroy(gameObject, 1);
        }



    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(raycastPoint.position, direction * raycastVision);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(behind.position, -direction * behindVision);
    }
}
