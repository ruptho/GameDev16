
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

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        robberController = robber.GetComponent<RobberController>();
        endText.text = "";
    }

    // before any physics calculation - put physics code here
    void FixedUpdate()
    {
        float moveVertical = Input.GetAxis("VerticalPolice");
        float moveHorizontal = Input.GetAxis("HorizontalPolice");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //rb.AddForce(movement * speed);
        rb.velocity = movement * speed;
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
