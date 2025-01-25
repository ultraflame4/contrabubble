using UnityEngine;

public interface IDriveableVehicle
{

    public Transform transform { get; }
    /// <summary>
    /// Moves the vehicle in said direction.
    /// </summary>
    /// <param name="accel">Vector to accelerate vehicle. </param>
    /// <param name="maxSpeed">Maximum speed of vehicle</param>
    public void Accelerate(Vector3 acceleration, float maxSpeed);

    /// <summary>
    /// Exits the vehicle.
    /// </summary>
    public void ExitVehicle(IVehiclePassenger passenger);
    /// <summary>
    /// Enter the vehicle.
    /// </summary>
    public void EnterVehicle(IVehiclePassenger passenger);
}
