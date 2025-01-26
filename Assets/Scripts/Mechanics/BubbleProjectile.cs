using Unity.Netcode;
using UnityEngine;

public class BubbleProjectile : NetworkBehaviour
{
    public float life = 1000;
    public float damage = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        life -= Time.deltaTime;
        if (life < 0){
            NetworkObject.Despawn();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Health health)) {
            health.DamageRpc(damage);
        }
    }
}
