using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamreFromPlayerView : MonoBehaviour
{
    public Transform target; // Reference to the player's Transform component

    private Vector3 offset; // Offset between the camera and the player

    private void Start()
    {
        // Calculate the initial offset between the camera and the player
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // Update the camera's position based on the player's position and offset
        transform.position = target.position + offset;

        // Align the camera's rotation with the player's rotation
        //transform.rotation = target.rotation;
    }
}
