using Player.Diver;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseStunDuration", menuName = "Scriptable Objects/IncreaseStunDuration")]
public class IncreaseStunDuration : PowerUp
{
    public float amount;
    public float duration;

    public override void Apply(GameObject target)
    {
        PowerUpManager manager = target.GetComponent<PowerUpManager>();
        if (manager != null) {
            manager.TrackPowerUp(this, target, duration);
        }
        throw new NotImplementedException();
    }

    public override void Remove(GameObject target)
    {
        throw new NotImplementedException();
    }
}
