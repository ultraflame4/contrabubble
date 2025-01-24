using UnityEngine;

public class BubbleStorage : MonoBehaviour
{
    [SerializeField] private float maxLimit = Mathf.Infinity;

    private float _bubbles;
    public float Bubbles
    {
        get { return _bubbles; }
        set
        {
            _bubbles = Mathf.Clamp(value, 0f, maxLimit);
        }
    }
}
