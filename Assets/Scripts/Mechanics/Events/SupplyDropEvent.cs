using NUnit.Framework;
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
    [SerializeField] int DropAmount = 10;

    private List<GameObject> atkPowerUps = new List<GameObject>();
    private List<GameObject> defPowerUps = new List<GameObject>();


    public void TriggerAtkSupplyDrop() 
    {

    }

    public void TriggerDefSupplyDrop() 
    { 
    
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
