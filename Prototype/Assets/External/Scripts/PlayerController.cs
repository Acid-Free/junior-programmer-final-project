using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject ballObj;
    [SerializeField] Transform targetTf;
    Rigidbody playerRb;
    Camera currentCam;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        currentCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = currentCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                targetTf.position = hit.point;
                GameObject ballTf;
                // 2 is the position of player head
                ballTf = Instantiate(ballObj, transform.position + Vector3.up, ballObj.transform.rotation);
                ballTf.GetComponent<BallController>().SetTarget(targetTf);
            }
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        playerRb.velocity = new Vector3(Input.GetAxis("Horizontal"), playerRb.velocity.y, Input.GetAxis("Vertical")) * speed;
    }
}
