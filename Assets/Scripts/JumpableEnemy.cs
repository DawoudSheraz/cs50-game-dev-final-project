﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpableEnemy : MonoBehaviour
{
    // The force applied upward when jumped on
    public float verticalThrust;

    // Invincibilty granted to the player if player does not jump on but collides directly with enemy
    public float invincibilityTime;

    // Score given to user after the enemy kill
    public float killScore = 0f;

    public int playerDamage;

    public AudioSource killSound;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.IsPlayerJumping() && player.IsPlayerDescending())
            {
                player.HandleJumpOnEnemy(verticalThrust);
                player.IncrementScore(killScore);
                killSound.Play();

                // Disable rendering to give effect that object has been removed
                GetComponentInParent<SpriteRenderer>().enabled = false;


                GameObject objectToDestroy;
                // Get parent object to destroy or object itself if not parent
                try
                {
                    objectToDestroy = transform.parent.gameObject;
                }
                catch (NullReferenceException)
                {
                    objectToDestroy = gameObject;
                }

                // Don't destroy until the audio has played
                Destroy(objectToDestroy, killSound.clip.length);
            }
            else if (!player.IsPlayerInvincible())
            {
                player.TakeDamage(playerDamage);
                player.GrantTemporaryInvincibility(invincibilityTime);
            }
        }
    }
}
