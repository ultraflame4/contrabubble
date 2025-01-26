using UnityEngine;
using Unity.Netcode;

public class BubbleStorage : NetworkBehaviour
{
    [SerializeField] private float maxLimit = Mathf.Infinity;

    private NetworkVariable<float> _bubbles = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public float Bubbles
    {
        get { return _bubbles.Value; }
        set
        {
            _bubbles.Value = Mathf.Clamp(value, 0f, maxLimit);
        }
    }
}
