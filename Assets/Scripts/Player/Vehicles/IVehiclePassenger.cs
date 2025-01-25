using UnityEngine;

public interface IVehiclePassenger {
    public void NotifyVehicleEntered(IDriveableVehicle vehicle, bool isDriver);
    public void NotifyVehicleExit();

}
