using UnityEngine;

public interface IVehiclePassenger
{
    public void NotifyVehicleEntered(IDriveableVehicle vehicle, bool isDriver) { }
    public void NotifyVehicleExit() { }
    public void SetAvailableVehicle(IDriveableVehicle vehicle) { }
    public void UnsetAvailableVehicle(IDriveableVehicle vehicle);
}
