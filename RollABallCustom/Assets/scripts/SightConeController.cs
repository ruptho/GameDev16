using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SightConeController : MonoBehaviour {

    public GameObject player; // our player
    public int sightRange; // this is not the light range!
    private Vector3 offset; // how far away is the camera
    public Light sightCone; // the light
    public int rayCount; // defines the "precision" of our collision detection

    private Vector3 initialRotation;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
        initialRotation = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        transform.position = player.transform.position + offset;
        Vector3 velo = (player.GetComponent<Rigidbody>()).velocity;

        if (velo != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velo);
            transform.Rotate(initialRotation);
        }

        doRayCasts();
    }

    private void doRayCasts()
    {
        Vector3 playerPos = player.transform.position;

        // WARNING: really dumb workaround for now because of the introduced rotation of the light
        // ... since I already spent 2 hours looking on vector methods
        // => i know it's stupid, you know it's stupid - let's just keep it that way for now
        transform.Rotate(-initialRotation);
        Vector3 centerForward = transform.forward;
        transform.Rotate(initialRotation);

        float lightAngle = sightCone.spotAngle;
        float lowerY = -lightAngle / 2;

        float interval = lightAngle / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            Vector3 direction = Quaternion.Euler(0, lowerY + interval * i, 0) * centerForward;
            castSingleRay(playerPos, direction);
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
                GameObject childForMinimap = hitObject.transform.GetChild(0).gameObject;

                if (!childForMinimap.activeInHierarchy) {
                    childForMinimap.SetActive(true);
                }
            }
        }

        // for debugging, makes ray visible
        //  Vector3 forward = direction * sightRange;
        // Debug.DrawRay(startingPos, forward, Color.red);
    }
}
