using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpableEnemy : MonoBehaviour
{
    // The force applied upward when jumped on
    public float verticalThrust;

    // Invincibilty granted to the player if player does not jump on but collides directly with enemy
    public float invincibilityTime;

    public AudioSource killSound;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.IsPlayerJumping() && player.IsPlayerDescending())
            {
                player.HandleJumpOnEnemy(verticalThrust);
                killSound.Play();

                // Disable rendering to give effect that object has been removed
                GetComponentInParent<SpriteRenderer>().enabled = false;
                // Don't destroy until the audio has played
                Destroy(transform.parent.gameObject, killSound.clip.length);
            }
            else if (!player.IsPlayerInvincible())
            {
                player.GrantTemporaryInvincibility(invincibilityTime);
            }
        }
    }
}
