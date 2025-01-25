using UnityEngine;

public interface IDriveableVehicle
{

    public Transform transform { get; }
    /// <summary>
    /// Moves the vehicle in said direction.
    /// </summary>
    /// <param name="direction">Vector of movement. Magnitude may be normalised by the vehicle.</param>
    public void MoveDirection(Vector3 direction);

    /// <summary>
    /// Exits the vehicle.
    /// </summary>
    public void ExitVehicle(IVehiclePassenger passenger);
    /// <summary>
    /// Enter the vehicle.
    /// </summary>
    public void EnterVehicle(IVehiclePassenger passenger);
}
