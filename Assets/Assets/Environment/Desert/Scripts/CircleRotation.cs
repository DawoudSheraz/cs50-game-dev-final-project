using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotation : MonoBehaviour
{

    public float radius;

    public float moveSpeed = 1f;

    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        angle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        angle += 1;
        if (angle > 360)
        {
            angle = 0;
        }

        float radianAngle = angle * Mathf.Deg2Rad * moveSpeed; 
        float xPos = radius * Mathf.Cos(radianAngle);
        float yPos = radius * Mathf.Sin(radianAngle);

        transform.position = new Vector2(xPos, yPos);
    }
}
