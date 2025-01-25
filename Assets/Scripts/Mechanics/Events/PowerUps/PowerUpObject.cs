using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    public PowerUp powerUp;

    private void OnTriggerEnter(Collider other)
    {
        PowerUpManager pum = other.GetComponent<PowerUpManager>();

        if (pum != null) 
        {
            if (!pum.isPoweredUp) 
            {
                if (powerUp.personal && other.CompareTag("Player")) {
                    powerUp.Apply(other.gameObject);
                    gameObject.SetActive(false);
                    Debug.Log("personal power up triggered");
                }
                else if (!powerUp.personal && other.CompareTag("Submarine")) {
                    powerUp.Apply(other.gameObject);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
