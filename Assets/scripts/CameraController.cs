using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // variables defined in editor/inspector
    public GameObject player;

    List<Light> LightsToRemove = new List<Light>();

    // helper variables 
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
        GameObject SightCone = player.transform.FindChild("SightCone").gameObject;
        GameObject Spotlight1 = SightCone.transform.FindChild("Spotlight1").gameObject;
        GameObject Spotlight2 = SightCone.transform.FindChild("Spotlight2").gameObject;
        GameObject DirectionalLight = player.transform.FindChild("Directional light").gameObject;
        GameObject Light1 = player.transform.FindChild("Light1").gameObject;

        LightsToRemove.Add(SightCone.GetComponent<Light>());
        LightsToRemove.Add(Spotlight1.GetComponent<Light>());
        LightsToRemove.Add(Spotlight2.GetComponent<Light>());
        LightsToRemove.Add(DirectionalLight.GetComponent<Light>());

        LightsToRemove.Add(Light1.GetComponent<Light>());



    }


    // runs after all items have been processed
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }

    private void OnPreCull()
    {
       
        foreach (Light light in LightsToRemove)
        {
            light.enabled = true;
        }
    }

    private void OnPreRender()
    {
        foreach (Light light in LightsToRemove)
        {
            light.enabled = true;
        }
    }

    private void OnPostRender()
    {
        foreach (Light light in LightsToRemove)
        {
            light.enabled = false;
        }

    }

}
