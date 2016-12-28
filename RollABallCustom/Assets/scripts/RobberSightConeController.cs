
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class RobberSightConeController : MonoBehaviour
{
    // variables defined in editor/inspector
    public GameObject player; // our player
    public int sightRange; // how far the player can "see" (this is not the light range but rather the collision detection range)
    private Vector3 offset; // how far away is the light
    public Light sightCone; // the real light source
    public int rayCount; // defines the "precision" of our collision detection

    // helper variables
    private Vector3 lookDirection; // where the player is currently looking according to its movement
    private HashSet<GameObject> currVisibleObjects; // objects the player can currently see (should disappear on minimap if no longer in sight)


    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
        currVisibleObjects = new HashSet<GameObject>();
    }

    void FixedUpdate()
    {
        // reset seen objects 
        disableCurrVisibleObjects();
    }

    void Update()
    {
        transform.position = player.transform.position + offset;
        Vector3 velo = (player.GetComponent<Rigidbody>()).velocity;

        if (velo != Vector3.zero)
        {
            lookDirection = velo.normalized;
            transform.rotation = Quaternion.LookRotation(velo);
        }

        // do raycasts to get visible elements
        doRayCasts();
    }

    void LateUpdate()
    {
        // display seen objects
        enableCurrVisibleObjects();
    }

    private void disableCurrVisibleObjects()
    {
        foreach (GameObject go in currVisibleObjects)
        {
            hideMinimapObject(go.transform.GetChild(0).gameObject);
        }

        currVisibleObjects.Clear(); // now reset the objects
    }

    private void enableCurrVisibleObjects()
    {
        foreach (GameObject go in currVisibleObjects)
        {
            displayMinimapObject(go.transform.GetChild(0).gameObject);
        }
    }

    private void doRayCasts()
    {
        Vector3 playerPos = player.transform.position;
        float lightAngle = sightCone.spotAngle;
        float lowerY = -lightAngle / 2;

        float interval = lightAngle / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            Vector3 direction = Quaternion.Euler(0, lowerY + interval * i, 0) * lookDirection;
            castSingleRay(playerPos, direction);
        }
    }

    private void castSingleRay(Vector3 startingPos, Vector3 direction)
    {
        // return ordered hit objects, this is important - "because walls"
        RaycastHit[] raycastHits = Physics.RaycastAll(new Ray(startingPos, direction), sightRange).OrderBy(h => h.distance).ToArray();

        // iterate over all hit objects ordered by distance
        foreach (RaycastHit hit in raycastHits)
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.transform.tag == "wall")
            {
                // if the ray hit a wall, we won't see further.
                break;
            }
            else
            {
                // these objects will be "discovered" and are NOT moving = ALWAYS visible on minimap
                if (hitObject.tag == "pickup")
                {
                    // first child of pickups is minimap object
                    displayMinimapObject(hitObject.transform.GetChild(0).gameObject);
                }
                else
                {
                    // these objects will be "discovered" and ARE moving = only temporarly visible
                    if (hitObject.tag == "police")
                    {
                        currVisibleObjects.Add(hitObject);
                    }
                }
            }
        }

        // for debugging, makes rays visible
        Vector3 forward = direction * sightRange;
        Debug.DrawRay(startingPos, forward, Color.red);
    }

    // --------- simple helpers ---------
    private void displayMinimapObject(GameObject minimapObject)
    {
        if (!minimapObject.activeInHierarchy)
        {
            minimapObject.SetActive(true);
        }
    }

    private void hideMinimapObject(GameObject minimapObject)
    {
        if (minimapObject.activeInHierarchy)
        {
            minimapObject.SetActive(false);
        }
    }
}
