using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float fireballSpeed = 2.5f;
    public float fireballDamage = 0f;
    public bool facingRight;
    private Rigidbody2D rb;
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 facing = facingRight ? Vector3.right : Vector3.left;
        Vector2 direction = gameObject.transform.rotation * facing;
        rb.AddForce(direction * fireballSpeed);
    }

    public void setRotation(bool direction)
    {
        facingRight = direction;
        Vector3 currentScale = gameObject.transform.localScale;
        if (!facingRight)
        {
            currentScale.x = -1;
        }
        else
        {
            currentScale.x = 1;
        }
        gameObject.transform.localScale = currentScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.name == "dummy")
            {
                DummyController dummy = collision.gameObject.GetComponent<DummyController>();
                if (dummy != null)
                {
                    dummy.takeDamage(fireballDamage);
                }
            }
            Destroy(this.gameObject);
        }
        else
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            ClientSend.SendDamage(player.id, fireballDamage);
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }
}