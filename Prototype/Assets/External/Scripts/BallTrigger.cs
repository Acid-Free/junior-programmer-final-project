using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    Rigidbody ballRb;
    [HideInInspector]public bool groundEntered;

    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("ground"))    
        {
            groundEntered = true;           
        }
    }
}
