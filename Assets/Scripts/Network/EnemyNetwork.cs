using Unity.Netcode;
using UnityEngine;

public class EnemyNetwork : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool _usingServerAuth;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;

    private NetworkVariable<EnemyNetworkState> _enemyState;

    private Vector3 _posVel;
    private float _rotVel;

    private void Awake()
    {
        var permission = _usingServerAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _enemyState = new(writePerm: permission);
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
        var state = new EnemyNetworkState
        {
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles
        };

        if (IsServer || !_usingServerAuth)
        {
            _enemyState.Value = state;
        }
        else
        {
            TransmitStateServerRpc(state);
        }
    }

    [ServerRpc]
    private void TransmitStateServerRpc(EnemyNetworkState state)
    {
        _enemyState.Value = state;
    }

    private void ConsumeState()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _enemyState.Value.Position, ref _posVel, _cheapInterpolationTime);
        transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _enemyState.Value.Rotation.y, ref _rotVel, _cheapInterpolationTime), 0);
    }

    struct EnemyNetworkState : INetworkSerializable
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
}
