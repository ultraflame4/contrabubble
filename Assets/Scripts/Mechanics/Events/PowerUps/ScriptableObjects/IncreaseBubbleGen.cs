using Player.Diver;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseBubbleGen", menuName = "Scriptable Objects/IncreaseBubbleGen")]
public class IncreaseBubbleGen : PowerUp
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
