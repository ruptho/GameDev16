using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // variables defined in editor/inspector
    public GameObject player;

    // helper variables 
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
    }


    // runs after all items have been processed
    void LateUpdate()
    {
        Debug.Log("player: " + player);
        Debug.Log("offset: " + offset);
        transform.position = player.transform.position + offset;
        Debug.Log("player position: " + player.transform.position);
    }
}
