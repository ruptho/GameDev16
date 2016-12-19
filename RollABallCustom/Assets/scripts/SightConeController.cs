using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightConeController : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
    }


    void FixedUpdate()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        transform.Rotate(new Vector3(0, moveHorizontal, 0));
    }

    // runs after all items have been processed
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

        Vector3 velo = (player.GetComponent<Rigidbody>()).velocity;

        if (velo != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(velo);
        }
    }
}
