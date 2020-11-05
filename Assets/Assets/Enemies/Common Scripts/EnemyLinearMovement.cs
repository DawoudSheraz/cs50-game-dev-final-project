using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLinearMovement : MonoBehaviour
{
    /* Script to move an object to and fro horizontally over a specified distance. */

    public float moveDistance;

    public float moveSpeed;

    [SerializeField]
    private bool movingRight;

    private float distanceCovered = 0f;

    // Determine direction based on the face
    void Start()
    {
        if (!movingRight)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (distanceCovered <= moveDistance)
        {
            distanceCovered += 1;
            transform.position = new Vector2(transform.position.x + ((movingRight ? 1 : -1) * moveSpeed), transform.position.y);
        }
        else if (distanceCovered > moveDistance)
        {
            distanceCovered = 0f;
            movingRight = !movingRight;
            Flip();
        }

    }

    private void Flip()
    {
        Vector2 scale = transform.localScale;
        transform.localScale = new Vector2(-1 * scale.x, scale.y);

    }
}
