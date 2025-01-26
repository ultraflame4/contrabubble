using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bubble : MonoBehaviour
{
    [SerializeField] private int bubbleValue = 1;
    [SerializeField] private Collider col;

    public void Deactivate()
    {
        col.enabled = false;
    }

    public void Collected(BubbleStorage BSscript) 
    {
        BSscript.Bubbles += bubbleValue;
        gameObject.SetActive(false);
        col.enabled = true;
    }
}
