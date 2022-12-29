using Unity.Netcode;
using UnityEngine;

public class PlayerLook : NetworkBehaviour
{
    public Transform PlayerCamera;
    public Vector2 Sensitivities;

    private Vector2 _xyRotation;

    //public override void OnNetworkSpawn()
    //{
    //    if (!IsOwner) Destroy(this);
    //}

    void Update()
    {
        if (!IsOwner) return;

        Vector2 mouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        _xyRotation.x -= mouseInput.y * Sensitivities.y;
        _xyRotation.y += mouseInput.x * Sensitivities.x;

        _xyRotation.x = Mathf.Clamp(_xyRotation.x, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, _xyRotation.y, 0f);
        PlayerCamera.localEulerAngles = new Vector3(_xyRotation.x, 0f, 0f);
    }
}
