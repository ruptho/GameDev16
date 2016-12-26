﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadController : MonoBehaviour {
    public GameObject player; // our player

    private Vector3 offset;

    // Use this for initialization
    void Start () {
        offset = transform.position - player.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position + offset;
    }
}
