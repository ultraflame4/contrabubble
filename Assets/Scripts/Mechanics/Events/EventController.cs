using System.Collections;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class EventController : NetworkBehaviour
{
    [Header("Scripts")]
    [SerializeField] private SupplyDropEvent SDEvent;
    [SerializeField] private BEARaidEvent BEAREvent;

    [Tooltip("How Frequent an event occured in seconds")]
    [SerializeField] private Vector2 eventFrequencyMinMax = new Vector2(45f, 75f);

    public bool debug_triggerEvent = false;
    public bool debug_triggerSPEvent = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!IsServer) return;

        if (SDEvent == null || BEAREvent == null) {
            Debug.LogWarning("Event Scripts are not set in inspector");
        }

        StartCoroutine(EventCaller());
    }

    private IEnumerator EventCaller()
    {
        while (true) {
            yield return new WaitForSeconds(Random.Range(eventFrequencyMinMax.x, eventFrequencyMinMax.y));

            TryTriggerEvent();
        }
    }

    void Update()
    {
        if (debug_triggerEvent) {
            debug_triggerEvent = false;
            TryTriggerEvent();
        }
        if (debug_triggerSPEvent) {
            debug_triggerSPEvent = false;
            StartCoroutine(SDEvent.TriggerAtkSupplyDrop());
        }
    }

    private void TryTriggerEvent()
    {
        // Check triggerability of events
        bool isAtkSupplyDropTriggerable = SDEvent.CheckAvailability(0);
        bool isDefSupplyDropTriggerable = SDEvent.CheckAvailability(1);
        bool isBEARaidTriggerable = true; // BEARaid is always available

        var availEvents = new List<int>();


        if (isAtkSupplyDropTriggerable) {
            availEvents.Add(0);
        }

        if (isDefSupplyDropTriggerable) {
            availEvents.Add(1);
        }
        if (isBEARaidTriggerable) {
            availEvents.Add(2);
        }
        // Select random element from availEvents
        if (availEvents.Count > 0) {
            int selectedEvent = availEvents[Random.Range(0, availEvents.Count)];
            switch (selectedEvent) {
                case 0:
                    Debug.Log("Triggerring Event: TriggerAtkSupplyDrop");
                    StartCoroutine(SDEvent.TriggerAtkSupplyDrop());
                    break;
                case 1:
                    Debug.Log("Triggerring Event: TriggerDefSupplyDrop");
                    StartCoroutine(SDEvent.TriggerDefSupplyDrop());
                    break;
                case 2:
                    Debug.Log("Triggerring Event: TriggerBEARaid");
                    BEAREvent.TriggerBEARaid();
                    break;

                default:
                    Debug.Log("Sum tingjs wong");
                    break;
            }
        }
    }
}

