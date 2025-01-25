using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private int bubbleValue = 1;

    public void Collected(BubbleStorage BSscript) 
    {
        BSscript.Bubbles += bubbleValue;
        Destroy(this);
    }
}
