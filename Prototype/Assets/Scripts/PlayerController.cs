using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        playerRb.velocity = new Vector3(Input.GetAxis("Horizontal"), playerRb.velocity.y,  Input.GetAxis("Vertical")) * moveSpeed;
    }
}
