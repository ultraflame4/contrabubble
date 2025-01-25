using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDropEvent : MonoBehaviour
{
    [Header("Powerup Prefab")]
    [SerializeField] private GameObject powerUp;

    [Header("Event Parameters")]
    [Tooltip("Amount of powerups on the field required for the event to stop triggering")]
    [SerializeField] int maxPowerUps = 8;
    [Tooltip("Amount of powerups to drop per supply run")]
    [SerializeField] int dropAmount = 10;
    [SerializeField] float dropLength = 100f;
    [SerializeField] Vector3 startPosition = new Vector3(50, 5, 0);

    [Header("PowerUp Scriptable Objects")]
    [SerializeField] private List<PowerUp> atkSObjects = new List<PowerUp>();
    [SerializeField] private List<PowerUp> defSObjects = new List<PowerUp>();

    private List<GameObject> atkPowerUps = new List<GameObject>();
    private List<GameObject> defPowerUps = new List<GameObject>();

    private bool startPlane;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && startPlane == false) 
        {
            StartCoroutine(TriggerAtkSupplyDrop());
        }
        if (Input.GetMouseButtonDown(1) && startPlane == false) 
        {
            StartCoroutine(TriggerDefSupplyDrop());
        }
    }

    public IEnumerator TriggerAtkSupplyDrop() 
    {
        startPlane = true;
        float distBetweenDrop = dropLength / dropAmount;
        float currentDist = distBetweenDrop * Random.Range(0.6f, 1.2f);
        int dropped = 0;

        while (dropped <= dropAmount - 1) 
        {
            transform.Translate(Vector3.left * dropLength * Time.deltaTime * 0.1f);
            currentDist += Mathf.Abs(Vector3.left.x * dropLength * Time.deltaTime * 0.1f);
            if (currentDist > distBetweenDrop) 
            {
                GameObject newPowerUp = GetInactivePowerUpFromPool(0);
                newPowerUp.GetComponent<PowerUpObject>().powerUp = atkSObjects[Random.Range(0, atkSObjects.Count)];
                newPowerUp.transform.position = transform.position;
                newPowerUp.SetActive(true); 
                currentDist = 0;
                dropped++;
            }
            yield return null;
        }
        transform.position = startPosition;
        startPlane = false;
    }

    public IEnumerator TriggerDefSupplyDrop() 
    {
        startPlane = true;
        float distBetweenDrop = dropLength / dropAmount;
        float currentDist = distBetweenDrop * Random.Range(0.6f, 1.2f);
        int dropped = 0;

        while (dropped <= dropAmount - 1) 
        {
            transform.Translate(Vector3.left * dropLength * Time.deltaTime * 0.1f);
            currentDist += Mathf.Abs(Vector3.left.x * dropLength * Time.deltaTime * 0.1f);
            if (currentDist > distBetweenDrop) 
            {
                GameObject newPowerUp = GetInactivePowerUpFromPool(1);
                newPowerUp.GetComponent<PowerUpObject>().powerUp = defSObjects[Random.Range(0, defSObjects.Count)];
                newPowerUp.transform.position = transform.position;
                newPowerUp.SetActive(true);
                currentDist = 0;
                dropped++;
            }
            yield return null;
        }
        transform.position = startPosition;
        startPlane = false;
    }

    private GameObject GetInactivePowerUpFromPool(int i)
    {
        List<GameObject> pool = (i == 0) ? pool = atkPowerUps : defPowerUps;

        foreach (GameObject powerUp in pool)
        {
            if (!powerUp.activeInHierarchy) {
                return powerUp;
            }
        }

        GameObject newPowerUp = Instantiate(powerUp);
        pool.Add(newPowerUp);
        newPowerUp.SetActive(false);
        return newPowerUp;
    }

    public bool CheckAvailability(int i) 
    {
        if (i == 0 && atkPowerUps.Count < maxPowerUps) 
        {
            return true;
        }
        else if (i == 1 && defPowerUps.Count < maxPowerUps) 
        {
            return true;
        }
        return false;
    }
}
