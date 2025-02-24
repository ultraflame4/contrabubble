using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Submarine : NetworkBehaviour
{

    public uint max_players = 2;
    [Tooltip("How many bubbles the submarine generates per second.")]
    public int bubble_gen_rate = 1;

    [Tooltip("Recoil force of bubble gun")]
    public float recoilForce = 15f;

    [Tooltip("How many bubbles it will cost to fire 1 projectile")]
    public float projectileCost = 2f;

    public GameObject projectilePrefab;

    private NetworkList<NetworkBehaviourReference> passengers;

    private BubbleStorage bubbleStore;
    public Transform doorMarker;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        passengers = new();
        TryGetComponent(out bubbleStore);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Only generate bubbles on the server
        if (IsServer) {

            if (rb.linearVelocity.magnitude < 0.03f) {
                bubbleStore.Bubbles += bubble_gen_rate * Time.deltaTime;
            }

        }
    }

    [Rpc(SendTo.Server)]
    public void ShootProjectileRpc(Vector3 direction)
    {
        if (bubbleStore.Bubbles < projectileCost) return;
        bubbleStore.Bubbles -= projectileCost;
        rb.AddForce(-(direction.normalized) * recoilForce, ForceMode.Force);
        var instance = Instantiate(projectilePrefab);
        instance.transform.position = transform.position;
        var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
        var instanceRb = instance.GetComponent<Rigidbody>();
        instanceRb.AddForce(direction, ForceMode.Impulse);
        // TODO SPAWN PROJECTILE
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
        rb.linearVelocity = Vector3.ClampMagnitude(velocity, Mathf.Max(rb.linearVelocity.magnitude, maxSpeed));

    }


    public void EnterVehicle(VehiclePassenger passenger)
    {
        EnterVehicleRpc(passenger);
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

    public void ExitVehicle(VehiclePassenger passenger)
    {
        ExitVehicleRpc(passenger);
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
