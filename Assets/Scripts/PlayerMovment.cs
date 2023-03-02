using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpFprce = 14f;
    private enum MovmentState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Raw make it less slitly;
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveSpeed * dirX, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpFprce);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovmentState state;

        if (dirX > 0f)
        {
            state = MovmentState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovmentState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovmentState.idle;
        }

        if(rb.velocity.y > .1f)
        {
            state = MovmentState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovmentState.falling;
        }


        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}