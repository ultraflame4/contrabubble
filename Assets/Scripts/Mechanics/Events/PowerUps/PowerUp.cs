using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public bool attack;
    public bool personal;
    public abstract void Apply(GameObject target);
    public abstract void Remove(GameObject target);
}
