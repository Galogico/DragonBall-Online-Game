using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class EnemyBasis : NetworkBehaviour
{
    [SerializeField] NetworkVariable<int> health = new NetworkVariable<int>(value:10);
    // [SerializeField] NetworkVariable<int> health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Owner);
    public AudioClip punchSound;

    void Start()
    {
        
    }

    void Update()
    {
        if (health.Value <= 0 && IsOwner)
        {
            SetHealthServerRPC(10);
            transform.position = new Vector3(0, 1, 0 );
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage){
        health.Value -= damage;
        PlayPunchSoundClientRPC();
    }

    [ClientRpc]
    void PlayPunchSoundClientRPC(){
        AudioManager.instance.PlaySFXClip(punchSound, gameObject.transform);
    }

    public int GetHealth(){
        return health.Value;
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetHealthServerRPC(int newHealth){
        health.Value = newHealth;
    }


        
}
