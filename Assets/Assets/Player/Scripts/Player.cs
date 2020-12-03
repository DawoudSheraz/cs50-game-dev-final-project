using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private static readonly float DEFAULT_SPEED = 5f;
    private static readonly int MAX_HEALTH = 10;

    private Animator animator;
    private Rigidbody2D playerBody;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isJumping = false;
    private bool isLanding = false;
    private bool onGround = false;
    private bool isFacingRight = true;
    private bool isInvincible = false;

    [SerializeField]
    private Transform groundTransform;

    [SerializeField]
    private HealthBar healthBar;

    [SerializeField]
    private float groundCheckRadius = 0.1f;

    public float moveSpeed = DEFAULT_SPEED;
    public float jumpSpeed = DEFAULT_SPEED;
    public float score = 0;
    public int currentHealth;

    public LayerMask[] groundLayers;
    public AudioSource audioSource;
    public AudioClip[] sounds;


    void Awake()
    {
        // Setup components in Awake so that Scene configuration(also using Awake) can call player methods using these components
        animator = GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody2D>();
        healthBar.SetMaxValue(MAX_HEALTH);
        healthBar.SetValue(MAX_HEALTH);
        currentHealth = MAX_HEALTH;
        score = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();
        CheckIsPlayerLanding();
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
        moveSpeed = 9f;
        animator.SetFloat("playerSpeed", 3.1f);
    }

    // stop the run flow
    private void StopRun()
    {
        isRunning = false;
        moveSpeed = DEFAULT_SPEED;
        animator.SetFloat("playerSpeed", 1.3f);
    }


    private void PlayJumpSound()
    {
        audioSource.Stop();
        audioSource.clip = sounds[0];
        audioSource.Play();
    }

    // Start of Private Wrappers on different animator triggers
    private void SetJumpTrigger()
    {
        animator.SetTrigger("jump");
    }

    private void ResetJumpTrigger()
    {
        animator.ResetTrigger("jump");
    }


    private void SetLandingTrigger()
    {
       animator.SetTrigger("landing");
    }

    private void ResetLandingTrigger()
    {
        animator.ResetTrigger("landing");
    }
    // End of private Wrappers on different animator triggers


    void FixedUpdate()
    {
        MovePlayer();
        HandleRun();
        Jump();

        // Debug code to change scene to verify player is not destroyed
        if (Input.GetKey("return"))
        {
            SceneManager.LoadScene(1);
        }

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
            isFacingRight = xInput >= 0;
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

    private void CheckIsPlayerLanding()
    {
        // If player is falling and landing and ground booleans are unset
        if (!onGround && IsPlayerDescending() && !isLanding )
        {
            isLanding = true;
            animator.SetLayerWeight(1, 1);
            SetLandingTrigger();
        }
    }


    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            SetJumpTrigger();
            animator.SetLayerWeight(1, 1);
            isJumping = true;
            isLanding = false;
            onGround = false;
            playerBody.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
            PlayJumpSound();
        }
       
    }

    // Check if the player is on ground
    private void IsGrounded()
    {
        if (!onGround)
        {
            foreach (LayerMask groundLayer in groundLayers)
            {
                Collider2D collider = Physics2D.OverlapCircle(groundTransform.position, groundCheckRadius, groundLayer);

                if (collider != null)
                {
                    isJumping = false;
                    isLanding = false;
                    onGround = true;
                    animator.SetLayerWeight(1, 0);
                    ResetJumpTrigger();
                    ResetLandingTrigger();
                    break;

                }

            }
        }
    }


    // To be called when player can jump on certain enemies and get a small force
    // upon landing on them
    public void HandleJumpOnEnemy(float upThrust)
    {
        playerBody.AddForce(Vector2.up * upThrust, ForceMode2D.Impulse);
    }

    // Check if player is falling
    public bool IsPlayerDescending()
    {
        return playerBody.velocity.y < 0;
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

    // Reduce health and update health bar
    public void TakeDamage(int value)
    {
        currentHealth = Mathf.Max(currentHealth - value, 0);
        healthBar.SetValue(currentHealth);
    }

    public void IncrementScore(float value)
    {
        score += value;
    }

    public bool IsPlayerJumping()
    {
        return isJumping;
    }

    public bool IsPlayerInvincible()
    {
        return isInvincible;
    }

    // The configuration to set when a new scene is loaded
    // while retaining some previous values
    public void SceneStartConfiguration()
    {
        isJumping = false;

        StopRun();
        StopWalk();
        animator.SetLayerWeight(1, 0);
        ResetJumpTrigger();
        ResetLandingTrigger();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Whenever leave a tough surface, check if player is falling off the edge
        if(IsPlayerDescending() && onGround)
        {
            onGround = false;
        }
    }
}

