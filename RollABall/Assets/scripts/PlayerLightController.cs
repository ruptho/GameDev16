using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightController : MonoBehaviour {
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // runs after all items have been processed
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
