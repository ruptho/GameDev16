using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobberController : MonoBehaviour
{

    // variables defined in editor/inspector
    // public float speed; // initial speed of the player
    public float initialSpeed;
    public float strength;
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
    private Vector3 lookDirection;  // where the player is currently looking according to its movement
    private List<PoliceController> allPoliceControllers = new List<PoliceController>(); // the controller script for the police
    private List<GameObject> lootInventory = new List<GameObject>();
    private float currentSpeed;

    private KeyCode forwardKey;
    private KeyCode backKey;
    private KeyCode leftKey;
    private KeyCode rightKey;

    float moveVertical = 0.0f;
    float moveHorizontal = 0.0f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        foreach (Transform policeMan in allPoliceMan)
            allPoliceControllers.Add(policeMan.GetComponent<PoliceController>());

        // carriedCount = 0;
        endText.text = "";

        currentSpeed = initialSpeed;
    }

    // before any physics calculation - put physics code here
    void FixedUpdate()
    {
    }

    private void Update()
    {
        HandleInputs();
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * currentSpeed;

        Vector3 velo = (GetComponent<Rigidbody>()).velocity;

        if (velo != Vector3.zero)
        {
            lookDirection = velo.normalized;
        }

        // enable object dropping via space
        if (Input.GetKeyUp("space"))
        {
            if (lootInventory.Count > 0)
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
            pickupObject(other.gameObject);
        }
        else if (other.gameObject.CompareTag("safeArea"))
        {
            if (lootInventory.Count > 0)
            {
                //carriedObject.tag = "stolenObject";

                dropLootInventory();
                infoText.text = "You succesfully stole an object!";
                stolenObjectsText.text = "Stolen Objects: " + (stolenCount);


                if (stolenCount >= objectsToSteal)
                {
                    signalWin();
                    foreach (PoliceController policeController in allPoliceControllers)
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
        lootInventory.Add(go);
        // carriedObject = go;
        go.SetActive(false);
        // carriedCount++;
        infoText.text = "You picked up an object!";

        currentSpeed = initialSpeed * (1 - calculateSpeedLoss());

        Debug.Log("CURR SPEED: " + currentSpeed + " INIT: " + "LOSS: " + (1 - calculateSpeedLoss()));
    }

    private void dropLootInventory()
    {
        stolenCount += lootInventory.Count;

        while (lootInventory.Count > 0)
        {
            lootInventory[lootInventory.Count - 1].tag = "stolenObject";
            dropObject(lookDirection * 1.5f);
        }
    }

    private void dropObject(Vector3 offset)
    {
        GameObject droppedObject = lootInventory[lootInventory.Count - 1];
        lootInventory.RemoveAt(lootInventory.Count - 1);
        droppedObject.SetActive(true);
        droppedObject.transform.position = (transform.position + offset);
        droppedObject.transform.rotation = Quaternion.identity;
        //        carriedCount--;
        //        carriedObject = null;
        currentSpeed = initialSpeed * (1 - calculateSpeedLoss());
    }

    private float calculateSpeedLoss()
    {

        float mass = 0;

        foreach (GameObject go in lootInventory)
        {
            Rigidbody currentRb = go.GetComponent<Rigidbody>();
            mass += currentRb.mass;
        }

        float weight = mass * 9.81f; // In Newton

        Debug.Log("WEIGHT: " + weight + " STRENGTH: " + strength);

        float speedLoss = weight / strength;

        if (speedLoss > 1)
        {
            speedLoss = 1;
        }

        return speedLoss;
    }

    // this will be called by PoliceController
    public void signalLose()
    {
        infoText.text = "You got caught by the police!";
        endText.text = "You lose! :-(";
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

    public void SetPoliceMan(List<GameObject> allPoliceElements)
    {
        foreach (GameObject currPoliceElement in allPoliceElements)
        {
            Transform policeMan = currPoliceElement.transform.FindChild("Police");
            allPoliceMan.Add(policeMan);
        }
        foreach (Transform policeMan in allPoliceMan)
            allPoliceControllers.Add(policeMan.GetComponent<PoliceController>());
    }

    public void SetInputs(KeyCode forwardKey, KeyCode backKey, KeyCode leftKey, KeyCode rightKey)
    {
        this.forwardKey = forwardKey;
        this.backKey = backKey;
        this.leftKey = leftKey;
        this.rightKey = rightKey;
    }

    public void HandleInputs()
    {
        if (Input.GetKeyUp(forwardKey))
            moveVertical -= 1.0f;

        if (Input.GetKeyUp(backKey))
            moveVertical += 1.0f;

        if (Input.GetKeyUp(leftKey))
            moveHorizontal += 1.0f;

        if (Input.GetKeyUp(rightKey))
            moveHorizontal -= 1.0f;


        if (Input.GetKeyDown(forwardKey))
            moveVertical += 1.0f;

        if (Input.GetKeyDown(backKey))
            moveVertical -= 1.0f;

        if (Input.GetKeyDown(leftKey))
            moveHorizontal -= 1.0f;

        if (Input.GetKeyDown(rightKey))
            moveHorizontal += 1.0f;
    }
}