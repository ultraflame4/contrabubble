using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private List<GameObject> activeBoats = new List<GameObject>();

    public void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            TriggerBEARaid();
        }
    }

    public void TriggerBEARaid() 
    {
        StartCoroutine(SpawnBoats());
    }

    private IEnumerator SpawnBoats()
    {
        yield return null;
    }
}
