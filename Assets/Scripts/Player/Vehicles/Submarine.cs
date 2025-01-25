using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Submarine : MonoBehaviour, IDriveableVehicle {

    public uint max_players = 2;
    public List<IVehiclePassenger> passengers { get; private set; } = new List<IVehiclePassenger>();

    [CanBeNull]
    public IVehiclePassenger driver => passengers.FirstOrDefault(null);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void AddPlayer(IVehiclePassenger passenger) {

    }

    public void MoveDirection(System.Numerics.Vector3 direction) {
        throw new System.NotImplementedException();
    }

    public void EnterVehicle(IVehiclePassenger passenger) {
        if (passengers.Count == 2) {
            Debug.Log("Max player in submarine!");
            return;
        }
        bool willBeDriver = passengers.Count == 0;
        passengers.Add(passenger);
        passenger.NotifyVehicleEntered(this, willBeDriver);
    }
    public void ExitVehicle(IVehiclePassenger passenger) {
        passengers.Remove(passenger);
        passenger.NotifyVehicleExit();
    }

}
