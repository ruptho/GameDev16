
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobberController : MonoBehaviour
{

    // variables defined in editor/inspector
    public float speed; // initial speed of the player
    public float speedLostPerObject; // speed lost if the player carries a object (currently maxed to 1)
    public Text infoText; // UI element
    public Text stolenObjectsText;  // UI element
    public Text endText;  // UI element
    public GameObject robberHead; // the "modeled head of" the player
    public GameObject sightCone; // the "sight cone"/spotlight
    public int objectsToSteal; // how many object the robber has to steal until he wins
    public List<Transform> allPoliceMan = new List<Transform>();  // used to call functions for the police player (e.g. signalLose)

    // helper variables 
    private Rigidbody rb; // the "real" rigidbody/sphere
    private int carriedCount; // how many objects are carried currently (only 1 possible for now)
    private int stolenCount; // the amount of objects which has been definitely stolen (brought to safe area)
    private GameObject carriedObject; // the currently carried object
    private float carrySpeed; // the speed of the player while carrying objects
    private Vector3 lookDirection;  // where the player is currently looking according to its movement
    private List<PoliceController> allPoliceControllers = new List<PoliceController>(); // the controller script for the police

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        foreach (Transform policeMan in allPoliceMan)
            allPoliceControllers.Add(policeMan.GetComponent<PoliceController>());

        carriedCount = 0;
        // Todo: Fix this!
        //endText.text = "";
        carrySpeed = speed - speedLostPerObject;
    }

    // before any physics calculation - put physics code here
    void FixedUpdate()
    {
        float moveVertical = Input.GetAxis("VerticalRobber");
        float moveHorizontal = Input.GetAxis("HorizontalRobber");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //rb.AddForce(movement * speed); don't accelerate, just set the speed
        rb.velocity = movement * speed;

        Vector3 velo = (GetComponent<Rigidbody>()).velocity;

        if (velo != Vector3.zero)
        {
            lookDirection = velo.normalized;
        }
    }

    private void Update()
    {
        // enable object dropping via space
        if (Input.GetKeyUp("space"))
        {
            if (carriedCount > 0)
            {
                infoText.text = "You dropped an object!";

                // drop objet a bit behind robber
                dropObject(lookDirection * -1.5f);
            }
            else
            {
                infoText.text = "You can't drop something you don't carry!";
            }
        }
    }

    // triggers entering
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            // if no object is carried, pickup hit object
            if (carriedCount == 0)
            {
                pickupObject(other.gameObject);
            }
            else
            {
                infoText.text = "It's too heavy!\n(You can only hold one object!)";
            }
        }
        else if (other.gameObject.CompareTag("safeArea"))
        {
            if (carriedCount > 0)
            {
                carriedObject.tag = "stolenObject";
                dropObject(lookDirection * 1.5f);
                infoText.text = "You succesfully stole an object!";
                stolenObjectsText.text = "Stolen Objects: " + (++stolenCount);

                if (stolenCount >= objectsToSteal)
                {
                    signalWin();
                    foreach(PoliceController policeController in allPoliceControllers)
                      policeController.signalLose();
                }
            }
            else
            {
                infoText.text = "You don't hold an object you can steal!";
            }
        }
    }

    private void pickupObject(GameObject go)
    {
        carriedObject = go;
        go.SetActive(false);
        carriedCount++;
        infoText.text = "You picked up an object!";
        speed = carrySpeed;
    }

    private void dropObject(Vector3 offset)
    {
        carriedObject.SetActive(true);
        carriedObject.transform.position = (transform.position + offset);
        carriedObject.transform.rotation = Quaternion.identity;
        carriedCount--;
        carriedObject = null;
        speed = carrySpeed + speedLostPerObject; // speed us up again
    }

    // this will be called by PoliceController
    public void signalLose()
    {
        // Todo: Fix this!
        //infoText.text = "You got caught by the police!";
        //endText.text = "You lose! :-(";
        transform.gameObject.SetActive(false);
        robberHead.gameObject.SetActive(false);
        sightCone.gameObject.SetActive(false);

        if (carriedCount > 0)
        {
            dropObject(new Vector3(0, 0, 0)); // drop object at same position
        }
    }

    public void signalWin()
    {
        infoText.text = "You succesfully stole the required objects!";
        endText.text = "You Win! :-)";
    }

    public void setPoliceMan(List<GameObject> allPoliceElements)
    {
        foreach (GameObject currPoliceElement in allPoliceElements)
        {
            Transform policeMan = currPoliceElement.transform.FindChild("Police");
            allPoliceMan.Add(policeMan);
        }
        foreach (Transform policeMan in allPoliceMan)
            allPoliceControllers.Add(policeMan.GetComponent<PoliceController>());
    }
}
