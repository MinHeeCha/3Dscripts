using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Stop speed
        if (Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        // Direction animation
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // Walking animation
        if (Mathf.Abs(rigid.velocity.x) < 0.4)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        // Jump animation
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        //else
        //    anim.SetBool("isJumping", false);

    }

    void FixedUpdate()
    {
        // Move speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max speed
        if (Mathf.Abs(rigid.velocity.x) > maxSpeed)
            rigid.velocity = new Vector2(rigid.velocity.x > 0 ? maxSpeed : -maxSpeed, rigid.velocity.y);

        // Landing platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D raytHit = Physics2D.Raycast(rigid.position + (spriteRenderer.flipX ? Vector2.left : Vector2.right), Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (raytHit.collider != null)
            {
                if (raytHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }

    }
}
