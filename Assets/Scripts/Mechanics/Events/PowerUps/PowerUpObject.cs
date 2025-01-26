using Unity.Netcode;
using UnityEngine;

public class PowerUpObject : NetworkBehaviour
{
    public PowerUp powerUp;

    [Rpc(SendTo.Everyone)]
    void NotifyHideRpc()
    {
        gameObject.SetActive(false);
    }

    [Rpc(SendTo.Server)]
    void NotifyApplyRpc(NetworkObjectReference target)
    {
        NotifyHideRpc();
        powerUp.Apply(target);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out PowerUpManager pum)) {
            if (pum.isPoweredUp) return;


            if (powerUp.personal && !other.CompareTag("Player")) return;

            if (!powerUp.personal && !other.CompareTag("Submarine")) return;
            
            NotifyApplyRpc(other.gameObject);

        }
    }
}
