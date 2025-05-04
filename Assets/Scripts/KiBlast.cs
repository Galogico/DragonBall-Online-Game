using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KiBlast : NetworkBehaviour
{
    public float speed;
    public float lifeTime;
    Rigidbody rb;
    public AudioClip BlastSound;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lifeTime = lifeTime + Time.time;
        AudioManager.instance.PlaySFXClip(BlastSound, transform);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;

        if(lifeTime < Time.time && IsHost){
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
            try{
                print("hit");
                if(IsOwner){
                other.transform.GetComponent<EnemyBasis>().TakeDamageServerRpc(1);
                }
                if(IsHost){
                    Destroy(gameObject);
                }
            }catch{
                print("hitNot");
                return;
            }
    }
}
