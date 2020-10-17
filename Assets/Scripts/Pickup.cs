using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public bool isRotating;

	public float pickupScore;

	// Sound to play upon pickup
	public AudioSource pickupAudio;


	void Update () {

		if (isRotating)
		{
			transform.Rotate(0, 5f, 0, Space.World);
		}
	}


    void OnTriggerEnter2D(Collider2D collision)
    {
		pickupAudio.Play();
		collision.gameObject.GetComponent<Player>().IncrementScore(pickupScore);

		// Disable rendering to give effect that object has been removed
		GetComponent<SpriteRenderer>().enabled = false;
		// Don't destroy until the audio has played
		Destroy(gameObject, pickupAudio.clip.length);
    }
}
