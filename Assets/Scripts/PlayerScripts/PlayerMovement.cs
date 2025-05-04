using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    float orientation;
    public Transform CameraBase, model;
    public GameObject playercCamera;
    Rigidbody rb;
    float horizontal, vertical,speed;
    public float orgSpeed, orgFlyingSpeed, JumpForce;
    Animator anim;
    Vector3 vetores;
    public bool JumpMode = true;



    public bool grounded;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner){
            Destroy(playercCamera);
            enabled = false;
        }
    }
    void Update()
    {

        orientation = CameraBase.rotation.eulerAngles.y * Mathf.PI/180;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        // print(horizontal.ToString() + "    " + vertical.ToString());
        if(Input.GetKeyDown(KeyCode.E)){
            model.rotation = Quaternion.Euler(0, CameraBase.rotation.eulerAngles.y, 0);
        }
        JumpModes();
    }

    void FixedUpdate(){
        //Se não estiver carregando o ki
        if(!Input.GetMouseButton(1)){
            Walk();
            Jump();
            Fly();
        }else{
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            anim.SetBool("isMoving", false);
        }
    }

    void Walk(){
        float XVertical = vertical * Mathf.Sin(orientation);
        float YVertical = vertical * Mathf.Cos(orientation);
        float XHorizontal = horizontal * Mathf.Sin(orientation + Mathf.PI/2);
        float YHorizontal = horizontal * Mathf.Cos(orientation + Mathf.PI/2);

        vetores = new Vector3(XVertical + XHorizontal, 0 ,YVertical + YHorizontal).normalized;

        if(vetores != Vector3.zero){
            model.forward = Vector3.Slerp(model.forward, vetores, Time.deltaTime * 7);
            // print("chess");
        }

        rb.velocity = new Vector3(vetores.x * speed, rb.velocity.y, vetores.z*speed);

        //Animator Bullshit
        
        if(horizontal != 0 || vertical != 0){
            anim.SetBool("isMoving", true);
        }else{
            anim.SetBool("isMoving", false);
        }
    }

    void Fly(){
        if(JumpMode == false){
            rb.useGravity = false;
            Vector3 jump = new Vector3(0f, 2f, 0f);
            if(Input.GetKey(KeyCode.Space)){
                rb.AddForce(jump * JumpForce);
            }
            if(Input.GetKey(KeyCode.LeftShift)){
                rb.AddForce(jump * -JumpForce);
            }
        }
    }
    void Jump(){
        if(JumpMode == true){
            rb.useGravity = true;
            Vector3 jump = new Vector3(0f, 2f, 0f);
            if(Input.GetKey(KeyCode.Space) && grounded){
                rb.AddForce(jump * 2000);
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == 3){
            grounded = true;
            anim.SetBool("grounded", true);
            speed = orgSpeed; 
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.layer == 3){
            grounded = false;
            anim.SetBool("grounded", false);
            speed = orgFlyingSpeed;
        }
    }

    void JumpModes(){
        if(Input.GetKeyDown(KeyCode.Q)){
            if (JumpMode == true){
                JumpMode = false;
            }else{
                JumpMode = true;
            }
        }
        if(JumpMode == true){
            anim.SetBool("jumpMode", true);
        }else{
            anim.SetBool("jumpMode", false);
        }
    }
}
