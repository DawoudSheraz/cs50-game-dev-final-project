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
        // trigger coin pickup function if a helicopter collides with this
        //other.transform.parent.GetComponent<Player>().PickupCoin();
		Destroy(gameObject);
	}
}
