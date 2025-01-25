using Player.Diver;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAttackSpeed", menuName = "Scriptable Objects/IncreaseAttackSpeed")]
public class IncreaseAttackSpeed : PowerUp
{
    public float amount;
    public float duration;

    public override void Apply(GameObject target)
    {
        PowerUpManager manager = target.GetComponent<PowerUpManager>();
        if (manager != null) {
            manager.TrackPowerUp(this, target, duration);
        }
        target.GetComponent<DiverController>().shootDuration -= amount;
    }

    public override void Remove(GameObject target)
    {
        target.GetComponent<DiverController>().shootDuration += amount;
    }
}
