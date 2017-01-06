
using UnityEngine;
using UnityEngine.UI;

public class PoliceController : MonoBehaviour
{
    // variables defined in editor/inspector
    public float speed;
    public Text infoText; // UI element
    public Text endText;  // UI element
    public GameObject robber; // used to call functions for the robber player (e.g. signalLose)

    // helper variables 
    private Rigidbody rb; // the "real" rigidbody/sphere
    private RobberController robberController; // the controller script for the robber

    Animator anim;
    Vector3 movement;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        robberController = robber.GetComponent<RobberController>();
        endText.text = "";
    }

    // before any physics calculation - put physics code here
    void FixedUpdate()
    {
        float moveVertical = Input.GetAxis("VerticalPolice");
        float moveHorizontal = Input.GetAxis("HorizontalPolice");

      
        // Move the player around the scene.
        Move(moveHorizontal, moveVertical);

        Animating(moveHorizontal, moveVertical);
    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;
        rb.velocity = movement;
        //--rotate in direction of movement
        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
        if (h != 0f || v != 0f)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);

            // Set the player's rotation to this new rotation.
            rb.MoveRotation(newRotation);
        }


        // Move the player to it's current position plus the movement.
        rb.MovePosition(transform.position + movement);
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
}
