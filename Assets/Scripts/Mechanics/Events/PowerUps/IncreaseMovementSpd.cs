using Player.Diver;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMovementSpd", menuName = "Scriptable Objects/IncreaseMovementSpd")]
public class IncreaseMovementSpd : PowerUp
{
    public float amount;
    public float duration;

    public override void Apply(GameObject target) 
    {
        PowerUpManager manager = target.GetComponent<PowerUpManager>();
        if (manager != null) {
            manager.TrackPowerUp(this, target, duration);
        }
        target.GetComponent<DiverController>().movementSpeed += amount;
    }

    public override void Remove(GameObject target)
    {
        target.GetComponent<DiverController>().movementSpeed -= amount;
    }
}
