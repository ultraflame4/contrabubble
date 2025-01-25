using Player.Diver;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMovementSpd", menuName = "Scriptable Objects/IncreaseMovementSpd")]
public class IncreaseMovementSpd : PowerUp
{
    public float amount;
    public float duration;

    public override void Apply(GameObject target) 
    {
        target.GetComponent<DiverController>().movementSpeed += amount;
    }
}
