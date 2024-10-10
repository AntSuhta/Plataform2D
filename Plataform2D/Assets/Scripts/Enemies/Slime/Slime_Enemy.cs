using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Enemy : MonoBehaviour
{

    [Header("ColliderPoint")]
    public float radius;
    public Transform point;
    public LayerMask layer;
    [Header("Status//Anim")]
    private Animator anim;
    public float speed;
    public int health;
    [Header("Attack")]
    public int damage;


    Rigidbody2D rig;


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(speed, rig.velocity.y);
        OnCollider();
    }


    void OnCollider()
    {


        Collider2D hit = Physics2D.OverlapCircle(point.position, radius, layer);
        if (hit != null)
        {
            speed = -speed;
            if (transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }


            Debug.Log("Toquei a parede");
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    public void OnHit(int dmg)
    {

        anim.SetTrigger("takeDamage");
        health -= dmg;
        Debug.Log(dmg);
        if (health <= 0)
        {
            anim.SetTrigger("death");
            speed = 0;
            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().OnHit(damage);
        }
    }

}
