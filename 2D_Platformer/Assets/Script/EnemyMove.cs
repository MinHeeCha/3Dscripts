using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D colider;
    
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colider = GetComponent<BoxCollider2D>();

        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // Platform check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D raytHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (raytHit.collider == null)
        {
            Turn();
        }
    }

    // Recursive function
    void Think()
    {
        // Set next active
        nextMove = Random.Range(-1, 2);

        // Sprite animation
        anim.SetInteger("walkSpeed", nextMove);

        // Flip sprite
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;
        
        // Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 5);
    }

    public void OnDamaged()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Sprite Flip Y
        spriteRenderer.flipY = true;

        // Collider disable
        colider.enabled = false;

        // Die effect jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Destroy
        Invoke("DeActive", 5);
    }

    void DeAactive()
    {
        gameObject.SetActive(false);
    }
}
