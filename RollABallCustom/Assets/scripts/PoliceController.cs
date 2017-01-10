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


    Animator anim;
    Vector3 movement;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        foreach(Transform robber in allRobbers)
          allRobberControllers.Add(robber.GetComponent<RobberController>());

        endText.text = "";
    }

    // before any physics calculation - put physics code here
    void FixedUpdate()
    {
        // Move the player around the scene.
        Move(moveHorizontal, moveVertical);

        Animating(moveHorizontal, moveVertical);
    }

    void Update()
    {
        HandleInputs();
      

    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;
        rb.velocity = movement;
        //--rotate in direction of movement
       if (h != 0f || v != 0f)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);

            // Set the player's rotation to this new rotation.
            rb.MoveRotation(newRotation);
        }


        // Move the player to it's current position plus the movement.
        rb.MovePosition(transform.position + movement);
        Debug.Log("POLICEPOSITION: " + transform.position);
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;
        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
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
            Transform robber = currRobberElement.transform.FindChild("Robber_nic");
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
