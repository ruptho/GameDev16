using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log("Loading user preferences:");

        List<GameObject> allRobbers = new List<GameObject>();
        List<GameObject> allPoliceMan = new List<GameObject>();

        int numbRobbers = 1;
        int numbCops = 1;

        // Check if player preferences exist
        if (PlayerPrefs.HasKey("NumbRobbers") && PlayerPrefs.HasKey("NumbCops"))
        {
            numbRobbers = PlayerPrefs.GetInt("NumbRobbers");
            numbCops = PlayerPrefs.GetInt("NumbCops");
        }

        // Disable second Minimap if only one robber
        if (numbRobbers < 2)
        {
            GameObject.Find("MinimapLayerRight").SetActive(false);
            GameObject.Find("RobberText1").SetActive(false);
        }

        if (numbCops < 2)
            GameObject.Find("PoliceText1").SetActive(false);

        // Create all robbers in Scene
        for (int robberCounter = 0; robberCounter < numbRobbers; robberCounter++)
        {
            GameObject newRobber = (GameObject)Instantiate(Resources.Load("robber"));
            Transform newRobberTransform = newRobber.transform;
            newRobberTransform.SetParent(GameObject.Find("PlayerElements").transform, false);
            newRobberTransform.position = new Vector3(-10 + robberCounter * 20, 0, 0);
            newRobber.name = "RobberElements" + robberCounter;
            allRobbers.Add(newRobber);
        }

        // Create all cops in Scene
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
        for (int robberCounter = 0; robberCounter < allRobbers.Count; robberCounter++)
        {
            Transform robber = allRobbers[robberCounter].transform.FindChild("Robber_nic");
            Debug.Log(robber);
            RobberController currRobberController = robber.GetComponent<RobberController>();

            KeyCode forwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ForwardR" + robberCounter));
            KeyCode backKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("BackwardR" + robberCounter));
            KeyCode leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftR" + robberCounter));
            KeyCode rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightR" + robberCounter));

            currRobberController.infoText = GameObject.Find("RobberText" + robberCounter).transform.FindChild("RobberInfoText").GetComponent<Text>();
            currRobberController.endText = GameObject.Find("RobberText" + robberCounter).transform.FindChild("RobberEndText").GetComponent<Text>();
            currRobberController.stolenObjectsText = GameObject.Find("RobberText" + robberCounter).transform.FindChild("RobbedObjectCountText").GetComponent<Text>();
            currRobberController.SetInputs(forwardKey, backKey, leftKey, rightKey);
            Debug.Log("allPoliceman: " + allPoliceMan);
            currRobberController.SetPoliceMan(allPoliceMan);
        }

        // Assign each policeman to all robbers
        for (int policeCounter = 0; policeCounter < allPoliceMan.Count; policeCounter++)
        {
            Transform policeMan = allPoliceMan[policeCounter].transform.FindChild("Police_nic");
            Debug.Log("policeMan: " + policeMan);
            PoliceController currPoliceController = policeMan.GetComponent<PoliceController>();

            KeyCode forwardKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ForwardC" + policeCounter));
            KeyCode backKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("BackwardC" + policeCounter));
            KeyCode leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftC" + policeCounter));
            KeyCode rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightC" + policeCounter));

            currPoliceController.infoText = GameObject.Find("PoliceText" + policeCounter).transform.FindChild("PoliceInfoText").GetComponent<Text>();
            currPoliceController.endText = GameObject.Find("PoliceText" + policeCounter).transform.FindChild("PoliceEndText").GetComponent<Text>();
            currPoliceController.SetInputs(forwardKey, backKey, leftKey, rightKey);
            currPoliceController.SetRobbers(allRobbers);
        }

        setUpCameras(allRobbers, allPoliceMan);
    }

    /**
     * Handles the correct placement of the split screen aspects.
     */
    public void setUpCameras(List<GameObject> allRobberElements, List<GameObject> allPoliceElements)
    {
        float robberCameraWidth = 1.0f / (float)allRobberElements.Count;
        for (int robberCounter = 0; robberCounter < allRobberElements.Count; robberCounter++)
        {
            Camera robberCamera = allRobberElements[robberCounter].GetComponentInChildren<Camera>();
            robberCamera.rect = new Rect(robberCameraWidth * robberCounter, 0.5f, robberCameraWidth, 0.5f);
        }

        float policeCameraWidth = 1.0f / (float)allPoliceElements.Count;
        for (int policeCounter = 0; policeCounter < allPoliceElements.Count; policeCounter++)
        {
            Camera policeCamera = allPoliceElements[policeCounter].GetComponentInChildren<Camera>();
            policeCamera.rect = new Rect(policeCameraWidth * policeCounter, 0.0f, policeCameraWidth, 0.5f);
        }
    }
}
