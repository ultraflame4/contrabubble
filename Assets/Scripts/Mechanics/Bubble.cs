using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Collider))]
public class Bubble : NetworkBehaviour
{
    [SerializeField] private int bubbleValue = 1;
    [SerializeField] private Collider col;

    public void Collected(BubbleStorage BSscript) 
    {
        BSscript.SetBubblesRPC(BSscript.Bubbles + bubbleValue);
        DestroyRPC();
    }

    [Rpc(SendTo.Server)]
    public void DestroyRPC()
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }

    [Rpc(SendTo.Everyone)]
    public void DeactivateRPC()
    {
        col.enabled = false;
    }
}
