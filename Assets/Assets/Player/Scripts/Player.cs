using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody playerBody;

    private bool isWalking = false;
    private bool isRunning = false;
    private bool isFacingRight = true;

    public float moveSpeed = 3f;

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
    private void startWalk()
    {
        isWalking = true;
        animator.SetFloat("playerSpeed", 1.3f);
    }

    // stop the walk flow
    private void stopWalk()
    {
        isWalking = false;
        animator.SetFloat("playerSpeed", 0);
    }

    void FixedUpdate()
    {
        float xInput = Input.GetAxis("Horizontal");
        if (xInput != 0)
        {
            this.startWalk();
        }
        else if (isWalking)
        {
            this.stopWalk();
        }
        movePlayer(xInput);

    }

    // Move player with correct facing
    private void movePlayer(float xInput)
    {
        playerBody.velocity = new Vector2(xInput * moveSpeed, playerBody.velocity.y);
    
        if(isFacingRight && xInput < 0 || !isFacingRight && xInput > 0)
        {
            Vector2 scale = transform.localScale;
            transform.localScale = new Vector2(scale.x * -1, scale.y);
            isFacingRight = xInput < 0 ? false : true;
        }
    }
}
