using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public float maxHealth = 100f;
    public NetworkVariable<float> currentHealth;

    void Awake()
    {
        currentHealth = new(maxHealth);
    }

    [Rpc(SendTo.Server)]
    public void Damage(int amt)
    {
        currentHealth.Value -= amt;
    }
}
