using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BubbleSpawner : NetworkBehaviour
{
    [Header("Bubble Types")]
    [SerializeField] private BubbleType[] bubbleTypes;
    [System.Serializable]
    private struct BubbleType
    {
        public GameObject prefab;
        [Range(0f, 100f)] public float spawnChance;
        [HideInInspector] public List<GameObject> pool;
    }

    [Header("Spawn Parameters")]
    [SerializeField] private int maxBubbles = 100;
    [SerializeField] private Vector2 playFieldSize = new Vector2(100f, 100f);
    [SerializeField] private float spawnRate = 10f;
    [SerializeField] private float minSpawnDistance = 1f;

    [Header("Spawn Velocity")]
    [SerializeField] private Vector2 minimum = new Vector2(-1, 0);
    [SerializeField] private Vector2 maximum = new Vector2(1, 1);
    [SerializeField] private Vector2 spawnDuration = new Vector2(0.75f, 1.2f);

    private int bubbleCount = 0;
    private float timeElasped, currentWaitDuration = 0f;

    private void Start() 
    {
        if (!IsServer) return;

        float sum = 0f;

        for (int i = 0; i < bubbleTypes.Length; i++)
        {
            if (bubbleTypes[i].prefab == null) 
                Debug.LogWarning($"bubble type {i} prefab is not set in inspector");

            sum += bubbleTypes[i].spawnChance;
        }

        if (sum != 100) 
            Debug.LogWarning("Spawn Chances Do not add up to 100");
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

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(playFieldSize, 1f);
        Gizmos.DrawSphere(-playFieldSize, 1f);
    }

    private GameObject GetInactiveBubbleFromPool(int index) 
    {
        if (bubbleTypes[index].pool == null)
            bubbleTypes[index].pool = new List<GameObject>();

        // Find an inactive bubble in the pool
        foreach (GameObject bubble in bubbleTypes[index].pool) 
        {
            if (!bubble.activeInHierarchy) 
            {
                return bubble;
            }
        }

        // If no inactive bubbles, create a new one
        GameObject newBubble = Instantiate(bubbleTypes[index].prefab, transform);
        newBubble.GetComponent<NetworkObject>()?.Spawn(true);
        bubbleTypes[index].pool.Add(newBubble);
        newBubble.SetActive(false);
        return newBubble;
    }

    private void SpawnBubbles()
    {
        for (int i = 0; i < spawnRate; i++) 
        {
            if (bubbleCount >= maxBubbles) break;

            Vector3 spawnPosition = GetValidSpawnPosition();

            if (spawnPosition == Vector3.zero) continue;

            GameObject newBubble = GetInactiveBubbleFromPool(GetBubbleIndexByChance());

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

    private int GetBubbleIndexByChance() 
    {
        float randomValue = Random.Range(0f, 100f);
        float currentChance = 0f;

        for (int i = 0; i < bubbleTypes.Length; i++)
        {
            currentChance += bubbleTypes[i].spawnChance;
            if (randomValue >= currentChance) continue;
            return i;
        }

        return bubbleTypes.Length - 1;
    }
}
