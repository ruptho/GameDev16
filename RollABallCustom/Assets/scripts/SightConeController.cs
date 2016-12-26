using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SightConeController : MonoBehaviour {

    public GameObject player;
    public int sightRange; // this is not the light range!
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

    private void Update()
    {
        transform.position = player.transform.position + offset;

        Vector3 velo = (player.GetComponent<Rigidbody>()).velocity;

        if (velo != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velo);
        }

    }

    // runs after all items have been processed
    void LateUpdate()
    {
        Vector3 playerPos = player.transform.position;
        ArrayList directions = new ArrayList();

        Vector3 centerForward = transform.forward;
        directions.Add(centerForward);
        Vector3 lower = Quaternion.Euler(0, -45, 0) * centerForward;
        //directions.Add(lower);
        Vector3 higher = Quaternion.Euler(0, 45, 0) * centerForward;
       // directions.Add(higher);

        doRayCasts(playerPos, directions);
    }

    private void doRayCasts(Vector3 startingPos, ArrayList directions)
    {
        foreach(Vector3 direction in directions)
        {
            castSingleRay(startingPos, direction);
        }
    }

    private void castSingleRay(Vector3 startingPos, Vector3 direction)
    {
        // return ordered hit objects, this is important - "because walls"
        RaycastHit[] raycastHits = Physics.RaycastAll(new Ray(startingPos, direction), sightRange).OrderBy(h => h.distance).ToArray();

        foreach (RaycastHit hit in raycastHits)
        {
            GameObject hitObject = hit.transform.gameObject;
            
            if(hitObject.transform.tag == "wall")
            {
                break;
            }
            else if (hitObject.tag == "pickup")
            {
                hitObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        Vector3 forward = direction * sightRange;
        Debug.DrawRay(startingPos, forward, Color.red);
    }
}
