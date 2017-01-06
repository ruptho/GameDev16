
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PoliceController : MonoBehaviour
{
    // variables defined in editor/inspector
    public float speed;
    public Text infoText; // UI element
    public Text endText;  // UI element
    public List<Transform> allRobbers = new List<Transform>(); // used to call functions for the robber player (e.g. signalLose)
    float moveVertical = 0.0f; 
    float moveHorizontal = 0.0f;

    private KeyCode forwardKey;
    private KeyCode backKey;
    private KeyCode leftKey;
    private KeyCode rightKey;

    // helper variables 
    private Rigidbody rb; // the "real" rigidbody/sphere
    private List<RobberController> allRobberControllers = new List<RobberController>(); // the controller script for the robber



    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        foreach(Transform robber in allRobbers)
          allRobberControllers.Add(robber.GetComponent<RobberController>());

        endText.text = "";
    }

    void Update()
    {
        HandleInputs();
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //rb.AddForce(movement * speed);
        rb.velocity = movement * speed;
    }

    // before any physics calculation - put physics code here
    void FixedUpdate()
    {
    }

    // collision detection with rigid bodies
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("robber"))
        {
            signalWin();
            Debug.Log("RobbrControllers: " + allRobberControllers.Count);
            foreach(RobberController robberController in allRobberControllers)
              robberController.signalLose();
        }
    }

    // this will be called by RobberController
    public void signalLose()
    {
        infoText.text = "You let the robber steal the objects he wanted!";
        endText.text = "You lose! :-(";
    }

    public void signalWin()
    {
        infoText.text = "You caught the robber!";
        endText.text = "You win! :-)";
    }

    public void SetRobbers(List<GameObject> allRobberElements)
    {
        foreach(GameObject currRobberElement in allRobberElements)
        {
            Transform robber = currRobberElement.transform.FindChild("Robber");
            allRobbers.Add(robber);
        }

        foreach (Transform robber in allRobbers)
            allRobberControllers.Add(robber.GetComponent<RobberController>());
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
