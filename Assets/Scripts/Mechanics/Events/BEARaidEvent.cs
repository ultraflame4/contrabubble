using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BEARaidEvent : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject diverPrefab;
    [SerializeField] private GameObject boatPrefab;

    [Header("Event Parameter")]
    [SerializeField] private int noOfBoats = 3;
    [SerializeField] private int noOfDiversPerBoats = 6;
    [SerializeField] private float eventDuration = 30f;
    [Tooltip("Distance from the centre of GameObject the boats are allowed to stop at")]
    [SerializeField] private float dockableArea = 50f;
    [Tooltip("Distance from the dockableArea where the boats will instantiate")]
    [SerializeField] private float spawnDist = 10f;
    [SerializeField] private float boatSpeed = 5f;
    [SerializeField] private float diverDropInterval = 1.5f;
    [SerializeField] private float boatSpawnInterval = 1f;
    [SerializeField] private float minDistBetweenBoats = 10f;

    private List<GameObject> activeBoats = new List<GameObject>();
    private List<Vector3> dockingPositions = new List<Vector3>();

    public void TriggerBEARaid() 
    {
        StartCoroutine(SpawnBoats());
    }

    private IEnumerator SpawnBoats()
    {
        for (int i = 0; i < noOfBoats; i++) 
        {
            // Choose a random side (left or right edge)
            float xSpawnPosition = transform.position.x + (Random.value > 0.5f ? dockableArea + spawnDist : -(dockableArea + spawnDist));
            Vector3 spawnPosition = new Vector3(xSpawnPosition, 0, 0);

            // Instantiate boat and assign a random docking position
            GameObject boat = Instantiate(boatPrefab, spawnPosition, Quaternion.identity);
            activeBoats.Add(boat);

            Vector3 dockingPosition = GetDockPosition(boat);
            bool positionFound = false;
            int attempts = 0;

            while (!positionFound && attempts < 100) 
            {
                positionFound = true;
                foreach (Vector3 existingPosition in dockingPositions) 
                {
                    if (Vector3.Distance(dockingPosition, existingPosition) < minDistBetweenBoats) 
                    {
                        positionFound = false;
                        dockingPosition = GetDockPosition(boat);
                        break;
                    }
                }

                attempts++;
            }

            if (!positionFound) {
                Debug.LogWarning("Could not find a valid docking position after maximum attempts.");
            }

            dockingPositions.Add(dockingPosition);
            StartCoroutine(MoveBoat(boat, dockingPosition));

            yield return new WaitForSeconds(boatSpawnInterval);
        }
    }

    private Vector3 GetDockPosition(GameObject boat)
    {
        Vector3 dockingPosition;

        if (boat.transform.position.x > transform.position.x) 
        {
            dockingPosition = new Vector3(
                    transform.position.x + Random.Range(transform.position.x, transform.position.x + dockableArea),
                    0,
                    0
                );
        }
        else 
        {
            dockingPosition = new Vector3(
                    transform.position.x + Random.Range(transform.position.x, transform.position.x - dockableArea),
                    0,
                    0
                );
        }
        return dockingPosition;
    }

    private IEnumerator MoveBoat(GameObject boat, Vector3 dockingPosition)
    {
        while (Vector3.Distance(boat.transform.position, dockingPosition) > 0.5f) {
            boat.transform.position = Vector3.MoveTowards(boat.transform.position, dockingPosition, boatSpeed * Time.deltaTime);
            yield return null;
        }

        // Start dropping divers
        StartCoroutine(DropDivers(boat));

        // Wait for event duration
        yield return new WaitForSeconds(eventDuration);

        // Recall divers
        StartCoroutine(RecallDivers(boat));

        //if divers havent return after 10 seconds just leave
        yield return new WaitForSeconds(10f);

        // Move boat back and destroy it
        StartCoroutine(ReturnBoat(boat));
    }

    private IEnumerator DropDivers(GameObject boat)
    {
        for (int i = 0; i < noOfDiversPerBoats; i++) 
        {
            Vector3 diverSpawnPosition = boat.transform.position + new Vector3(0, -1f, 0);
            GameObject diver = Instantiate(diverPrefab,diverSpawnPosition, Quaternion.identity, boat.transform);
            yield return new WaitForSeconds(diverDropInterval);
        }
        yield return null;
    }

    private IEnumerator RecallDivers(GameObject boat)
    {
        foreach (Transform child in boat.gameObject.transform) 
        {
            //some logic for diver to return to boat before disabling
            child.gameObject.SetActive(false);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator ReturnBoat(GameObject boat)
    {
        List<GameObject> boatsToReturn = new List<GameObject>(activeBoats);
        activeBoats.Clear(); // Clear the list before moving boats

        Vector3 exitPosition = new Vector3(boat.transform.position.x > 0 ? dockableArea + spawnDist : -(dockableArea + spawnDist), 0, 0);

        while (Vector3.Distance(boat.transform.position, exitPosition) > 0.5f) 
        {
            boat.transform.position = Vector3.MoveTowards(boat.transform.position, exitPosition, boatSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(boat);
    }
}
