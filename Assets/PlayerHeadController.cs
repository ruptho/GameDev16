
using UnityEngine;

public class PlayerHeadController : MonoBehaviour
{

    // variables defined in editor/inspector
    public GameObject player; // our player

    // helper variables 
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
