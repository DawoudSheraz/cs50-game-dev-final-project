using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    public float invincibilityTime;

    void OnTriggerEnter2D(Collider2D collision)
    {
    collision.gameObject.GetComponent<Player>().GrantTemporaryInvincibility(invincibilityTime);
    Destroy(gameObject);
    }
}
