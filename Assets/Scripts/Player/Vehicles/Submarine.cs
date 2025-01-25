using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Submarine : NetworkBehaviour
{

    public uint max_players = 2;
    private NetworkList<NetworkBehaviourReference> passengers;


    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        passengers = new();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnNetworkSpawn()
    {
        passengers.OnListChanged += (e) => {
            Debug.Log("New passenger!");
            Debug.Log("Count: " + passengers.Count);
        };
    }

    [Rpc(SendTo.Server)]
    public void AccelerateRpc(Vector3 acceleration, float maxSpeed)
    {
        var velocity = rb.linearVelocity;
        velocity += acceleration;
        rb.linearVelocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }

    [Rpc(SendTo.Server)]
    public void EnterVehicleRpc(NetworkBehaviourReference passenger)
    {
        if (passengers.Count == 2) {
            Debug.Log("Max player in submarine!");
            return;
        }
        bool willBeDriver = passengers.Count == 0;
        passengers.Add(passenger);
        if (passenger.TryGet(out VehiclePassenger p)) {
            if (willBeDriver) {
                // NetworkObject.ChangeOwnership(p.NetworkObject.OwnerClientId);
            }
            p.NotifyEnteredVehicleRpc(this, willBeDriver);
        }
    }

    [Rpc(SendTo.Server)]
    public void ExitVehicleRpc(NetworkBehaviourReference passenger)
    {
        passengers.Remove(passenger);
        if (passenger.TryGet(out VehiclePassenger p)) {
            p.NotifyExitedVehicleRpc(this);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out VehiclePassenger passenger);
        if (passenger == null) return;
        passenger.nearestSub = this;
    }


    public void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out VehiclePassenger passenger);
        if (passenger == null) return;
        if (passenger.nearestSub == this) {
            passenger.nearestSub = null;
        }
    }

}
