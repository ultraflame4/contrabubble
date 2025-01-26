using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RapidFire", menuName = "Scriptable Objects/RapidFire")]
public class RapidFire : PowerUp
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
