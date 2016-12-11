using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        // Rotate the object around its local X axis at 1 degree per second
        transform.Rotate(new Vector3(15,30,45) * Time.deltaTime);
    }
}
