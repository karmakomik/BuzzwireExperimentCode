﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCollision : MonoBehaviour
{
    public GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Snap!");

        //gameController.GetComponent<GameControllerScript>().doControllerReattachOperations();
        //gameController.GetComponent<GameControllerScript>().stopMistakeFeedback();

    }
}
