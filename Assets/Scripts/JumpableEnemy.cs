using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpableEnemy : MonoBehaviour
{
    // The force applied upward when jumped on
    public float verticalThrust;

    public AudioSource killSound;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.IsPlayerJumping() && player.IsPlayerDescending())
            {
                player.HandleJumpOnEnemy(verticalThrust);
                killSound.Play();

                // Disable rendering to give effect that object has been removed
                GetComponent<SpriteRenderer>().enabled = false;
                // Don't destroy until the audio has played
                Destroy(gameObject, killSound.clip.length);
            }
            else if (!player.IsPlayerInvincible())
            {
                player.GrantTemporaryInvincibility(0.8f);
            }
        }
    }
}
