using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadConfiguration : MonoBehaviour
{
    /*
     * Start coordinates of the player in a scene
     */
    public Vector2 playerInitializationCoordinates;

    [SerializeField]
    private GameObject playerPrefab;

    void Start()
    {
        Player player = (Player)GameObject.FindObjectOfType<Player>();

        if (player)
        {
            print("Player object is present in the scene, Moving the object to initialization coordinates");
            player.transform.position = playerInitializationCoordinates;
        }
        else
        {
            Debug.Log("No player object found. Creating new player at the initialization coordinates");
            Instantiate(playerPrefab, playerInitializationCoordinates, Quaternion.identity);
        }
    }
}
