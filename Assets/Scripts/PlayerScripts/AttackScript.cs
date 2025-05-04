using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackScript : NetworkBehaviour
{
    public float attackCoolDown;
    float CDStamp = 0;
    float ki = 0;
    Animator anim;
    public int attacks;
    public bool canAttack;
    public GameObject KiParticle;
    Transform cameraTransform;
    HealthBarScript kiBar;
    [SerializeField]
    private GameObject ki_blast;

    // Normal Attack
    public float punchRange;


    // Start is called before the first frame update
    void Start()
    {
        ki = 0;
        attacks = 0;
        anim = transform.GetChild(0).GetComponent<Animator>();
        canAttack =true;
        cameraTransform = transform.GetChild(1);
        kiBar = GameObject.Find("HealthBar").GetComponent<HealthBarScript>();
        kiBar.SetMaxHealth(5);
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner){
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        kiBar.SetHealth(ki);
        if(Input.GetMouseButton(1)){
            ChargeKi();
        }else{
            NormalAttacks();
            KiParticle.SetActive(false);
        }

        if(attacks > 3){
            attacks = 3;
        }

        if(ki > 5){
            ki = 5;
        }
    }

    void NormalAttacks(){
        //Melee

        if(Input.GetMouseButtonDown(0)){
            attacks += 1;
        }
        if(canAttack && attacks > 0){
            Melee();
        }

        //KiBlasts

        if(Input.GetKeyDown(KeyCode.E) && canAttack && ki >= 1){

            KiBlast();
        }


        //CoolDown

        if(CDStamp < Time.time){
            canAttack = true;
        }
    }
    [ServerRpc]
    void CheckRayCastServerRPC(){
        RaycastHit hit = new RaycastHit();
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if(Physics.Raycast(origin, transform.GetChild(0).forward, out hit, punchRange)){
            print(hit);
            try{
                hit.transform.GetComponent<EnemyBasis>().TakeDamageServerRpc(1);
                
            }catch{
                return;
            }
            
        }
    }


    
    void Melee(){
        attacks -= 1;
            int attackSelector = UnityEngine.Random.Range(0,3);
            if(attackSelector == 0){
                anim.SetTrigger("attack");
                canAttack = false;
                CheckRayCastServerRPC();
                CDStamp = Time.time + attackCoolDown;
            }
            if(attackSelector == 1){
                anim.SetTrigger("attack2");
                canAttack = false;
                CheckRayCastServerRPC();
                CDStamp = Time.time + attackCoolDown;
            }
            if(attackSelector == 2){
                anim.SetTrigger("attack3");
                canAttack = false;
                CheckRayCastServerRPC();
                CDStamp = Time.time + attackCoolDown;
            }
    }
    void KiBlast(){
        ki -= 1;
        SpawnKiBlastServerRPC(cameraTransform.rotation);
        anim.SetTrigger("attack");
        canAttack = false;
        CDStamp = Time.time + attackCoolDown;
    }
    [Rpc(SendTo.Server)]
    void SpawnKiBlastServerRPC(Quaternion camRotation){
        var instance = Instantiate(ki_blast , new Vector3(transform.position.x + Mathf.Sin(camRotation.eulerAngles.y* Mathf.Deg2Rad), transform.position.y +1, transform.position.z + Mathf.Cos(camRotation.eulerAngles.y * Mathf.Deg2Rad) ), camRotation);
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }

    void ChargeKi(){
        KiParticle.SetActive(true);
        ki += Time.deltaTime;
    }
}
