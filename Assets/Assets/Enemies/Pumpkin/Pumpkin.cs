using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : MonoBehaviour
{

    /*
     * Left & Right transforms to check enemy bounds on a ground platform
     */
    [SerializeField]
    private Transform LeftTransform;

    [SerializeField]
    private Transform RightTransform;

    private Animator animator;

    private Rigidbody2D rigidBody;

    private bool isWalking = false;
    private bool isRunning = false;

    public bool isFacingRight;

    public float moveSpeed = 7f;


    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        Move();
    }

    private void StartWalk()
    {
        isWalking = true;
        animator.SetFloat("speed", 1.1f);
    }

    private void StopWalk()
    {
        isWalking = false;
        animator.SetFloat("speed", 0);
    }

    private void StartRun()
    {
        isRunning = true;
        moveSpeed = 7f;
        animator.SetFloat("speed", 2.1f);
    }

    private void StopRun()
    {
        isRunning = false;
        moveSpeed = 7f;
        animator.SetFloat("speed", 1.1f);
    }


    private void Move()
    {
        rigidBody.velocity = new Vector2((isFacingRight ? 1 : -1) * moveSpeed, rigidBody.velocity.y);

        if (!isWalking)
        {
            isWalking = true;
            StartWalk();
        }

    }

    private void Flip()
    {
        Vector2 scale = transform.localScale;
        transform.localScale = new Vector2(-1 * scale.x, scale.y);
        isFacingRight = !isFacingRight;
    }


    void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            BoxCollider2D collider = collision.collider.GetComponent<BoxCollider2D>();

            // If moving right and just about to reach the end of ground, flip
            if (isFacingRight && (RightTransform.position.x >= (collider.transform.position.x + collider.size.x + collider.offset.x)))
            {
                Flip();
            }

            // If moving left and about to reach the end, flip
            else if (!isFacingRight && RightTransform.position.x < collider.transform.position.x)
            {
                Flip();
            }

        }
    }
}

