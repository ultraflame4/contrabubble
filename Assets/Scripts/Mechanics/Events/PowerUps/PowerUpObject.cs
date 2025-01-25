using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    public PowerUp powerUp;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PowerUpManager>().isPoweredUp) 
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
