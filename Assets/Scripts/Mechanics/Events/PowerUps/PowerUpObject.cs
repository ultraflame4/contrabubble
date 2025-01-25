using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    public PowerUp powerUp;

    private void OnTriggerEnter(Collider other)
    {
        if (powerUp.personal && other.CompareTag("Player"))
        {
            powerUp.Apply(other.gameObject);
            gameObject.SetActive(false);
        }
        else if (!powerUp.personal && other.CompareTag("Submarine")) 
        {
            powerUp.Apply(other.gameObject);
            gameObject.SetActive(false);
        }
    }
}
