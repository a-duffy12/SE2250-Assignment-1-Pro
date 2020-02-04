using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player; // attach the camera to the player

    private Vector3 offset; // how far the camera is away from the player

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position; // offset is the distance the camera is from the players last previous position
    }

    // Runs every frame but is guaranteed to run after all objects have been updated
    void LateUpdate() 
    { 
        transform.position = player.transform.position + offset; // updates the camera position before each frame is rendered
    }
}
