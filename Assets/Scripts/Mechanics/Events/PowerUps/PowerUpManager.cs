using System.Collections;
using UnityEngine;
using static PowerUpManager;

public class PowerUpManager : MonoBehaviour
{
    [HideInInspector] public bool isPoweredUp;

    private PowerUp activePowerUp;
    private Coroutine powerUpCoroutine;
    private GameObject currentTarget;

    public void TrackPowerUp(PowerUp newPowerUp, GameObject target, float duration)
    {
        isPoweredUp = true;

        // Remove existing power-up before applying a new one
        if (activePowerUp != null) 
        {
            activePowerUp.Remove(currentTarget);
            StopCoroutine(powerUpCoroutine);
        }

        // Set new active power-up
        activePowerUp = newPowerUp;
        currentTarget = target;

        // Start expiration timer
        powerUpCoroutine = StartCoroutine(HandlePowerUpExpiration(duration));
    }

    private IEnumerator HandlePowerUpExpiration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Remove the power-up effect
        if (activePowerUp != null && currentTarget != null) 
        {
            activePowerUp.Remove(currentTarget);
        }

        // Clear active power-up
        activePowerUp = null;
        currentTarget = null;
        powerUpCoroutine = null;
        isPoweredUp = false;
    }
}
