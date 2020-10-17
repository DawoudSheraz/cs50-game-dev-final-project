using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public bool isRotating;

	public float pickupScore;


	void Update () {

		if (isRotating)
		{
			transform.Rotate(0, 5f, 0, Space.World);
		}
	}


    void OnTriggerEnter2D(Collider2D collision)
    {
		collision.gameObject.GetComponent<Player>().IncrementScore(pickupScore);
        Destroy(gameObject);
	}
}
