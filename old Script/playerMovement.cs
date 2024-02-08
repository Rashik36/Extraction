using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class playerMovement : MonoBehaviour {

    [SerializeField] private float speed;
    private Rigidbody rb;
    [SerializeField] private float jump;
    private bool canJump;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Transform groundCheck;

    void Start () {

        rb = GetComponent<Rigidbody>();
        Application.targetFrameRate = 165;

    }

     void Update () {

        rb.velocity  = new Vector3(Input.GetAxis("Horizontal") * speed,rb.velocity.y,Input.GetAxis("Vertical") * speed);
  
        if(Input.GetButtonDown("Jump") && GroundCheck()){
            rb.velocity = new Vector3(rb.velocity.x,jump,rb.velocity.z);
        }

        
     }

     bool GroundCheck(){
        return Physics.CheckSphere(groundCheck.position, 0.3f, mask);
     }

 }