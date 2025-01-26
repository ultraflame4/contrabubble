using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BubbleSpawner : NetworkBehaviour
{
    [Header("Bubble Prefabs")]
    [SerializeField] private GameObject largeBubblePrefab;
    [SerializeField] private GameObject mediumBubblePrefab;
    [SerializeField] private GameObject smallBubblePrefab;

    [Header("% Chance bubble variant will spawn")]
    [SerializeField] private float largeSpawnChance;
    [SerializeField] private float mediumSpawnChance;
    [SerializeField] private float smallSpawnChance;

    [Header("Spawn Parameters")]
    [SerializeField] private int maxBubbles = 100;
    [SerializeField] private Vector2 playFieldSize = new Vector2(100f, 100f);
    [SerializeField] private float spawnRate = 10f;
    [SerializeField] private float minSpawnDistance = 1f;

    [Header("Spawn Velocity")]
    [SerializeField] private Vector2 minimum = new Vector2(-1, 0);
    [SerializeField] private Vector2 maximum = new Vector2(1, 1);
    [SerializeField] private Vector2 spawnDuration = new Vector2(0.75f, 1.2f);

    private List<GameObject> largeBubblePool = new List<GameObject>();
    private List<GameObject> mediumBubblePool = new List<GameObject>();
    private List<GameObject> smallBubblePool = new List<GameObject>();

    private int bubbleCount = 0;
    private float timeElasped, currentWaitDuration = 0f;

    private void Start() 
    {
        if (!IsServer) return;

        if (largeBubblePrefab == null || mediumBubblePrefab == null || smallBubblePrefab == null) 
        {
            Debug.LogWarning("bubbles prefab is not set in inspector");
            return;
        }

        if (largeSpawnChance +  smallSpawnChance + mediumSpawnChance != 100) 
        {
            Debug.LogWarning("Spawn Chances Do not add up to 100");
            return;
        }
    }

    private void Update()
    {
        if (!IsServer) return;
        
        timeElasped += Time.deltaTime;

        if (timeElasped < currentWaitDuration) return;

        bubbleCount = GetComponentsInChildren<Bubble>().GetLength(0);
        SpawnBubbles();
        timeElasped = 0f;
        currentWaitDuration = Random.Range(spawnDuration.x, spawnDuration.y);
    }

    private GameObject GetInactiveBubbleFromPool(GameObject prefab) 
    {
        List<GameObject> pool = GetPoolForPrefab(prefab);

        // Find an inactive bubble in the pool
        foreach (GameObject bubble in pool) 
        {
            if (!bubble.activeInHierarchy) 
            {
                return bubble;
            }
        }

        // If no inactive bubbles, create a new one
        GameObject newBubble = Instantiate(prefab, transform);
        pool.Add(newBubble);
        newBubble.SetActive(false);
        return newBubble;
    }

    private List<GameObject> GetPoolForPrefab(GameObject prefab) 
    {
        if (prefab == largeBubblePrefab) return largeBubblePool;
        if (prefab == mediumBubblePrefab) return mediumBubblePool;
        return smallBubblePool;
    }

    private void SpawnBubbles()
    {
        for (int i = 0; i < spawnRate; i++) 
        {
            if (bubbleCount >= maxBubbles) break;

            Vector3 spawnPosition = GetValidSpawnPosition();

            if (spawnPosition == Vector3.zero) continue;

            GameObject bubblePrefab = GetBubblePrefabByChance();

            GameObject newBubble = GetInactiveBubbleFromPool(bubblePrefab);

            newBubble.transform.position = spawnPosition;
            newBubble.SetActive(true);

            Rigidbody rb = newBubble.GetComponent<Rigidbody>();

            if (rb == null)
            {
                Debug.LogWarning("No Rigidbody found on Bubble Prefab");
                continue;
            }

            rb.isKinematic = false;
            rb.useGravity = false;

            Vector2 spawnForce = new Vector2(
                Random.Range(minimum.x, maximum.x),
                Random.Range(minimum.y, maximum.y));
            
            rb.AddForce(spawnForce, ForceMode.Impulse);
        }
    }

    private Vector3 GetValidSpawnPosition() 
    {
        Vector3 spawnPosition;
        int attempts = 0;

        do 
        {
            // Generate random position within play field
            spawnPosition = new Vector3(
                Random.Range(-playFieldSize.x / 2f, playFieldSize.x / 2f),
                Random.Range(-playFieldSize.y / 2f, playFieldSize.y / 2f),
                0f
            );

            attempts++;

            // Prevent infinite loop
            if (attempts > 100) 
            {
                Debug.LogWarning("Could not find a valid spawn position after 100 attempts.");
                return Vector3.zero;
            }
        }
        while (!IsPositionValid(spawnPosition));

        return spawnPosition;
    }

    private bool IsPositionValid(Vector3 newPosition) 
    {
        // Find all existing bubbles that are children of this spawner
        Bubble[] existingBubbles = GetComponentsInChildren<Bubble>();

        // Check distance from all existing bubbles
        foreach (Bubble existingBubble in existingBubbles) 
        {
            float distance = Vector3.Distance(newPosition, existingBubble.transform.position);

            if (distance < minSpawnDistance) 
                return false;
        }

        return true;
    }

    private GameObject GetBubblePrefabByChance() 
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue < largeSpawnChance) 
            return largeBubblePrefab;
        else if (randomValue < largeSpawnChance + mediumSpawnChance) 
            return mediumBubblePrefab;
        else 
            return smallBubblePrefab;
    }
}
