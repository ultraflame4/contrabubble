using Player.Diver;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreasedCarryCapacity", menuName = "Scriptable Objects/IncreasedCarryCapacity")]
public class IncreasedCarryCapacity : PowerUp
{
    public float amount;
    public float duration;

    public override void Apply(GameObject target)
    {
        PowerUpManager manager = target.GetComponent<PowerUpManager>();
        if (manager != null) {
            manager.TrackPowerUp(this, target, duration);
        }
        target.GetComponent<BubbleStorage>().Bubbles += amount;
    }

    public override void Remove(GameObject target)
    {
        target.GetComponent<BubbleStorage>().Bubbles -= amount;
    }
}
