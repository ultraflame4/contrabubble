using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class EventController : NetworkBehaviour
{
    [Header("Scripts")]
    [SerializeField] private SupplyDropEvent SDEvent;
    [SerializeField] private BEARaidEvent BEAREvent;

    [Tooltip("How Frequent an event occured in seconds")]
    [SerializeField] private Vector2 eventFrequencyMinMax = new Vector2(45f, 75f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!IsServer) return;

        if (SDEvent == null || BEAREvent == null) 
        {
            Debug.LogWarning("Event Scripts are not set in inspector");
        }

        StartCoroutine(EventCaller());
    }

    private IEnumerator EventCaller() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(Random.Range(eventFrequencyMinMax.x, eventFrequencyMinMax.y));

            TryTriggerEvent();
        }
    }

    private void TryTriggerEvent()
    {
        // Check triggerability of events
        bool isAtkSupplyDropTriggerable = SDEvent.CheckAvailability(0);
        bool isDefSupplyDropTriggerable = SDEvent.CheckAvailability(1);
        bool isBEARaidTriggerable = true; // BEARaid is always available

        // Scenario 1: All events are triggerable (1/3 chance for each)
        if (isAtkSupplyDropTriggerable && isDefSupplyDropTriggerable && isBEARaidTriggerable) 
        {
            int randomEvent = Random.Range(0, 3);
            switch (randomEvent) 
            {
                case 0:
                    SDEvent.TriggerAtkSupplyDrop();
                    break;
                case 1:
                    SDEvent.TriggerDefSupplyDrop();
                    break;
                case 2:
                    BEAREvent.TriggerBEARaid();
                    break;
            }
        }
        // Scenario 2: Atk Supply Drop not triggerable
        else if (!isAtkSupplyDropTriggerable && isDefSupplyDropTriggerable && isBEARaidTriggerable) 
        {
            int randomEvent = Random.Range(0, 2);
            switch (randomEvent) 
            {
                case 0:
                    SDEvent.TriggerDefSupplyDrop();
                    break;
                case 1:
                    BEAREvent.TriggerBEARaid();
                    break;
            }
        }
        // Scenario 3: Def Supply Drop not triggerable
        else if (isAtkSupplyDropTriggerable && !isDefSupplyDropTriggerable && isBEARaidTriggerable) 
        {
            int randomEvent = Random.Range(0, 2);
            switch (randomEvent) 
            {
                case 0:
                    SDEvent.TriggerAtkSupplyDrop();
                    break;
                case 1:
                    BEAREvent.TriggerBEARaid();
                    break;
            }
        }
        // Scenario 4: Only BEARaid is available
        else if (!isAtkSupplyDropTriggerable && !isDefSupplyDropTriggerable && isBEARaidTriggerable) 
        {
            BEAREvent.TriggerBEARaid();
        }
        // Scenario 5: No events are triggerable
        else 
        {
            Debug.Log("Sum Ting Wong");
        }
    }
}

