using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Loading user preferences:");

        List<GameObject> allRobbers = new List<GameObject>();
        List<GameObject> allPoliceMan = new List<GameObject>();

        // Create all robbers in Scene
        int numbRobbers = PlayerPrefs.GetInt("NumbRobbers");

        // Disable second Minimap if only one robber
        if (numbRobbers < 2)
          GameObject.Find("MinimapLayerRight").SetActive(false);

        for (int robberCounter = 0; robberCounter < numbRobbers; robberCounter++)
        {
            GameObject newRobber = (GameObject)Instantiate(Resources.Load("robber"));
            Transform newRobberTransform = newRobber.transform;
            newRobberTransform.SetParent(GameObject.Find("PlayerElements").transform, false);
            newRobberTransform.position = new Vector3(-10 + robberCounter * 20,0,0);
            newRobber.name = "RobberElements" + robberCounter;

            allRobbers.Add(newRobber);
        }

        // Create all cops in Scene
        int numbCops = PlayerPrefs.GetInt("NumbCops");
        for (int copCounter = 0; copCounter < numbCops; copCounter++)
        {
            GameObject newCop = (GameObject)Instantiate(Resources.Load("police"));
            Transform newCopTransform = newCop.transform;
            newCopTransform.SetParent(GameObject.Find("PlayerElements").transform, false);
            newCopTransform.position = new Vector3(-10 + copCounter * 20, 0, -10);
            newCop.name = "PoliceElements" + copCounter;
            allPoliceMan.Add(newCop);     
        }

        // Assign each robber to all policeman
        foreach(GameObject robberElement in allRobbers)
        {
            Transform robber = robberElement.transform.FindChild("Robber");
            RobberController currRobberController = robber.GetComponent<RobberController>();
            currRobberController.setPoliceMan(allPoliceMan);
        }

        // Assign each policeman to all robbers
        foreach (GameObject policeElement in allPoliceMan)
        {
            Transform policeMan = policeElement.transform.FindChild("Police");
            PoliceController currPoliceController = policeMan.GetComponent<PoliceController>();
            currPoliceController.setRobbers(allRobbers);
        }

        setUpCameras(allRobbers, allPoliceMan);
    }
	
	// Update is called once per frame
	void Update () {
	}

    /**
     * Handles the correct placement of the split screen aspects.
     */
    public void setUpCameras(List<GameObject> allRobberElements, List<GameObject> allPoliceElements)
    {
        float robberCameraWidth = 1.0f / (float)allRobberElements.Count;
        for(int robberCounter = 0; robberCounter < allRobberElements.Count; robberCounter++)
        {
            Camera robberCamera = allRobberElements[robberCounter].GetComponentInChildren<Camera>();
            robberCamera.rect = new Rect(robberCameraWidth * robberCounter, 0.5f,robberCameraWidth, 0.5f);
        }

        float policeCameraWidth = 1.0f / (float)allPoliceElements.Count;
        for (int policeCounter = 0; policeCounter < allPoliceElements.Count; policeCounter++)
        {
            Camera policeCamera = allPoliceElements[policeCounter].GetComponentInChildren<Camera>();
            policeCamera.rect = new Rect(policeCameraWidth * policeCounter, 0.0f, policeCameraWidth, 0.5f);
        }
    }
}
