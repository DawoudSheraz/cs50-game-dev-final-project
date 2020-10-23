using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPlayerConfig : MonoBehaviour
{

    // If player has collided with the platform, set the player transform to the current object's transform
    void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.CompareTag("Player"))
        {

            collision.transform.SetParent(this.transform);
        }
    }

    // If player has exited the collision, remove the transform parent from the player
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
