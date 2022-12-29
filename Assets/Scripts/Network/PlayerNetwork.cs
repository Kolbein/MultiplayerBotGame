using KinematicCharacterController;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private bool _usingServerAuth;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;
    [SerializeField] private GameObject _camera;
    [SerializeField] private Transform _character;

    private NetworkVariable<PlayerNetworkState> _playerState;

    private Vector3 _posVel;
    private float _rotVel;

    private void Awake()
    {
        var permission = _usingServerAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _playerState = new(writePerm: permission);
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _camera.SetActive(true);
        }
        else
        {
            Destroy(_character.GetComponent<KinematicCharacterMotor>());
        }
    }

    void Update()
    {
        if (IsOwner)
            TransmitState();
        else
            ConsumeState();
    }

    private void TransmitState()
    {
        var state = new PlayerNetworkState
        {
            Position = _character.position,
            Rotation = _character.rotation.eulerAngles
            //Position = transform.position,
            //Rotation = transform.rotation.eulerAngles
        };

        if (IsServer || !_usingServerAuth)
        {
            _playerState.Value = state;
            //Debug.Log(OwnerClientId + " " + _playerState.Value.Position + "IsServer || !_usingServerAuth");
        }
        else
        {
            TransmitStateServerRpc(state);
            //Debug.Log(OwnerClientId + " " + _playerState.Value.Position + "TransmitStateServerRpc");
        }
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerNetworkState state)
    {
        _playerState.Value = state;
    }

    private void ConsumeState()
    {
        //_character.position = _playerState.Value.Position;
        _character.position = Vector3.SmoothDamp(_character.position, _playerState.Value.Position, ref _posVel, _cheapInterpolationTime);
        _character.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(_character.rotation.eulerAngles.y, _playerState.Value.Rotation.y, ref _rotVel, _cheapInterpolationTime), 0);
    }

    struct PlayerNetworkState : INetworkSerializable
    {
        private float _posX, _posY, _posZ;
        private short _rotY;

        internal Vector3 Position
        {
            get => new(_posX, _posY, _posZ);
            set
            {
                _posX = value.x;
                _posY = value.y;
                _posZ = value.z;
            }
        }

        internal Vector3 Rotation
        {
            get => new(0, _rotY, 0);
            set => _rotY = (short)value.y;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _posX);
            serializer.SerializeValue(ref _posY);
            serializer.SerializeValue(ref _posZ);
            serializer.SerializeValue(ref _rotY);
        }
    }

    //[ServerRpc]
    //private void TestServerRpc(ServerRpcParams serverRpcParams)
    //{
    //    Debug.Log("TestServerRpc " + OwnerClientId + " " + serverRpcParams.Receive.SenderClientId);
    //}

    //[ClientRpc]
    //public void TestClientRpc(ClientRpcParams clientRpcParams)
    //{
    //    Debug.Log("TestClientRpc");
    //}
}
