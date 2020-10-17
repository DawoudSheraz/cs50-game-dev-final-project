using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D playerBody;

    private static readonly float DEFAULT_SPEED = 5f;

    private bool isWalking = false;
    private bool isRunning = false;
    private bool isJumping = false;
    private bool isFacingRight = true;
    private bool isInvincible = false;

    [SerializeField]
    private Transform groundTransform;

    [SerializeField]
    private float groundCheckRadius = 0.1f;

    public float moveSpeed = DEFAULT_SPEED;
    public float jumpSpeed = DEFAULT_SPEED;

    public float score = 0;

    public LayerMask[] groundLayers;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody2D>();
        score = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();
        
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
        Jump();

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


    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        { 
            animator.SetTrigger("jump");
            animator.SetLayerWeight(1, 1);
            isJumping = true;
            playerBody.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
        }
       
    }

    // Check if the player has reached the ground when jumping
    private void IsGrounded()
    {
        if (isJumping)
        {
            foreach (LayerMask groundLayer in groundLayers)
            {
                Collider2D collider = Physics2D.OverlapCircle(groundTransform.position, groundCheckRadius, groundLayer);

                if (collider != null)
                {
                    isJumping = false;
                    animator.SetLayerWeight(1, 0);
                    animator.ResetTrigger("jump");
                    break;
                }

            }
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject gameObject = collider.gameObject;
        
        if(gameObject.tag == "Enemy")
        {
            if (playerBody.velocity.y < 0 && isJumping)
            {
                this.HandleJumpOnEnemy(gameObject.GetComponent<JumpableEnemy>().verticalThrust);
                Destroy(collider.gameObject);
            }
            else if(!isInvincible)
            {
                GrantTemporaryInvincibility(0.8f);   
            }
        }
    }

    // To be called when player can jump on certain enemies and get a small force
    // upon landing on them
    private void HandleJumpOnEnemy(float upThrust)
    {
        playerBody.AddForce(Vector2.up * upThrust, ForceMode2D.Impulse);
    }

    // Coroutine to make player temporary invincible
    IEnumerator MakeTemporaryInvincible(float duration)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color original = renderer.material.color;

        isInvincible = true;
        for (float i = 0f; i < duration; i += 0.4f)
        {
            renderer.material.color = new Color(255, 255, 255, 0.5f);
            yield return new WaitForSeconds(0.2f);
            renderer.material.color = original;
            yield return new WaitForSeconds(0.2f);
        }
        isInvincible = false;

    }

    // Wrapper to call coroutine
    public void GrantTemporaryInvincibility(float duration)
    {
        StartCoroutine("MakeTemporaryInvincible", duration);
    }

    public void IncrementScore(float value)
    {
        score += value;
    }
}

