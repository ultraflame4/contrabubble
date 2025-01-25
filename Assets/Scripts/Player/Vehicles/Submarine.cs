using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Submarine : MonoBehaviour, IDriveableVehicle
{

    public uint max_players = 2;
    public List<IVehiclePassenger> passengers { get; private set; } = new List<IVehiclePassenger>();

    [CanBeNull]
    public IVehiclePassenger driver => passengers.FirstOrDefault(null);

    Transform IDriveableVehicle.transform => transform;


    Rigidbody rb;

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Accelerate(Vector3 acceleration, float maxSpeed)
    {
        var velocity = rb.linearVelocity;
        velocity += acceleration;
        rb.linearVelocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }

    public void EnterVehicle(IVehiclePassenger passenger)
    {
        if (passengers.Count == 2) {
            Debug.Log("Max player in submarine!");
            return;
        }
        bool willBeDriver = passengers.Count == 0;
        passengers.Add(passenger);
        passenger.NotifyVehicleEntered(this, willBeDriver);
    }
    public void ExitVehicle(IVehiclePassenger passenger)
    {
        passengers.Remove(passenger);
        passenger.NotifyVehicleExit();
    }

    public void OnTriggerEnter(Collider other)
    {
        IVehiclePassenger passenger;
        other.TryGetComponent(out passenger);
        if (passenger == null) return;
        passenger.SetAvailableVehicle(this);
    }


    public void OnTriggerExit(Collider other)
    {
        IVehiclePassenger passenger;
        other.TryGetComponent(out passenger);
        if (passenger == null) return;
        passenger.UnsetAvailableVehicle(this);
    }

}
