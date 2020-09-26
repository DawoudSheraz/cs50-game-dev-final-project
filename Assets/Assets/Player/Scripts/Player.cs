using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody playerBody;

    private static readonly float DEFAULT_SPEED = 5f;

    private bool isWalking = false;
    private bool isRunning = false;
    private bool isFacingRight = true;

    public float moveSpeed = DEFAULT_SPEED;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Start the walk flow
    private void StartWalk()
    {
        isWalking = true;
        animator.SetFloat("playerSpeed", 1.3f);
    }

    // stop the walk flow
    private void StopWalk()
    {
        isWalking = false;
        animator.SetFloat("playerSpeed", 0);
    }

    // Start the run flow
    private void StartRun()
    {
        isRunning = true;
        moveSpeed = 7f;
        animator.SetFloat("playerSpeed", 3.1f);
    }

    // stop the run flow
    private void StopRun()
    {
        isRunning = false;
        moveSpeed = DEFAULT_SPEED;
        animator.SetFloat("playerSpeed", 1.3f);
    }

    void FixedUpdate()
    {
        MovePlayer();
        HandleRun();

    }

    // Move player with correct facing
    private void MovePlayer()
    {
        float xInput = Input.GetAxis("Horizontal");
        if (xInput != 0)
        {
            this.StartWalk();
        }
        else if (isWalking)
        {
            this.StopWalk();
        }

        playerBody.velocity = new Vector2(xInput * moveSpeed, playerBody.velocity.y);
    
        if(isFacingRight && xInput < 0 || !isFacingRight && xInput > 0)
        {
            Vector2 scale = transform.localScale;
            transform.localScale = new Vector2(scale.x * -1, scale.y);
            isFacingRight = xInput < 0 ? false : true;
        }
    }

    // Handle the running logic
    private void HandleRun()
    {
        // Player should already be in walk state to run
        if (isWalking)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                this.StartRun();
            }
            else if (isRunning)
            {
                this.StopRun();
            }

        }
        
    }
}
