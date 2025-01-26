using System;
using Unity.Netcode;
using UnityEngine;

public enum DeathAction{
    NONE,
    DESTROY,
    DISABLE
}

public class Health : NetworkBehaviour
{
    public float maxHealth = 100f;
    private NetworkVariable<float> _currentHealth;
    public float current => _currentHealth.Value;

    public DeathAction death_action = DeathAction.NONE;
    public event Action OnDamaged;
    public event Action OnDeath;

    void Awake()
    {
        _currentHealth = new(maxHealth);
    }

    [Rpc(SendTo.Everyone)]
    void SetActiveRpc(bool active){
        gameObject.SetActive(active);
    }


    [Rpc(SendTo.Server)]
    public void DamageRpc(float amt)
    {
        _currentHealth.Value -= amt;
        OnDamaged?.Invoke();

        if (current <= 0) {
            OnDeath?.Invoke();

            switch (death_action) {
                case DeathAction.NONE:
                    break;
                case DeathAction.DESTROY:
                    NetworkObject.Despawn();
                    break;
                case DeathAction.DISABLE:
                    SetActiveRpc(false);
                    break;
            }

        }
    }
}
