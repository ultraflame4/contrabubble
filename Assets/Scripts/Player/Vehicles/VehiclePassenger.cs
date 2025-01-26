using System;
using Unity.Netcode;
using UnityEngine;

public class VehiclePassenger : NetworkBehaviour
{

    public Submarine nearestSub;
    public Submarine submarine;
    public bool is_driver = false;

    public Action<bool> EnteredVehicle;
    public Action ExitedVehicle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    [Rpc(SendTo.Everyone)]
    public void NotifyEnteredVehicleRpc(NetworkBehaviourReference submarineBehavior, bool is_driver = false)
    {
        if (submarineBehavior.TryGet(out submarine)) {
            this.is_driver = is_driver;
            EnteredVehicle.Invoke(is_driver);
        }
    }

    [Rpc(SendTo.Everyone)]
    public void NotifyExitedVehicleRpc(NetworkBehaviourReference submarineBehavior)
    {
        this.submarine = null;
        this.is_driver = false;
        ExitedVehicle.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

        // if (Input.GetKeyDown(KeyCode.E)) {
        //     if (submarine != null) {

        //         submarine.ExitVehicleRpc(this);
        //     }
        //     else if (nearestSub != null) {
        //         nearestSub.EnterVehicleRpc(this);
        //     }

        // }
    }
}
