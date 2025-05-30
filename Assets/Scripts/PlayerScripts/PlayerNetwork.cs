using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerNetwork : NetworkBehaviour{



    // transferir posicao
    private readonly NetworkVariable<PlayerNetworkData> _netState = new(writePerm: NetworkVariableWritePermission.Owner);
    private Vector3 _vel;
    private float _rotVel;

    [SerializeField] private float _cheapInterpolationTime = 0.1f;

    void Update()
    {
        if(IsOwner){

            _netState.Value = new PlayerNetworkData(){
                Position = transform.position,
                Rotation = transform.GetChild(0).rotation.eulerAngles,
            };
        }else{
            transform.position = Vector3.SmoothDamp(transform.position, _netState.Value.Position, ref _vel, _cheapInterpolationTime);
            transform.GetChild(0).rotation = Quaternion.Euler(
                0,
                Mathf.SmoothDampAngle(transform.GetChild(0).rotation.eulerAngles.y, _netState.Value.Rotation.y, ref _rotVel, _cheapInterpolationTime),
                0
            );
        }
    }
}


struct PlayerNetworkData : INetworkSerializable{
    private float _x, _z, _y;
    private float _yRot;

    internal Vector3 Position{
        get => new Vector3(_x, _y, _z);
        set{
            _x = value.x;
            _y = value.y;
            _z = value.z;
        }
    }

    internal Vector3 Rotation{
        get => new Vector3(0, _yRot, 0);
        set => _yRot = value.y;
        
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter{
        serializer.SerializeValue(ref _x);
        serializer.SerializeValue(ref _y);
        serializer.SerializeValue(ref _z);
        serializer.SerializeValue(ref _yRot);
    }
}