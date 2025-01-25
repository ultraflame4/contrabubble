using Unity.Netcode;
using UnityEngine;

public class NetworkedPlayer : NetworkBehaviour
{

    float moveSpeed = 100;
    Vector3 moveInput;
    Rigidbody rb;

    VehiclePassenger passenger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        TryGetComponent(out passenger);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        if (passenger) {
            if (passenger.is_driver) {
                passenger.submarine.AccelerateRpc(moveInput.normalized * moveSpeed * Time.deltaTime, 7);

                return;
            }
        }

        rb.linearVelocity = moveInput.normalized * moveSpeed * Time.deltaTime;
    }
}
