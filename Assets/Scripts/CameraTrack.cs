using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{

    // Transform set to player transform
    private Transform playerTransform;

    // Minimum & Maximum Y value to clamp camera's y position
    [SerializeField]
    private float yMin;

    [SerializeField]
    private float yMax;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    // LateUpdate so that camera update is done after all Update() calls have been made
    void LateUpdate()
    {
        transform.position = new Vector3(
            playerTransform.position.x,
            Mathf.Clamp(playerTransform.position.y, yMin, yMax),
            transform.position.z
            );
    }
}
