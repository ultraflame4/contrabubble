using UnityEngine;
using Unity.Netcode;
using TMPro;

public class BubbleStorage : NetworkBehaviour
{
    [SerializeField] private float maxLimit = Mathf.Infinity;
    [SerializeField] private TextMeshProUGUI bubbleCountText;

    public NetworkVariable<float> _bubbles = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public float Bubbles
    {
        get { return _bubbles.Value; }
        set
        {
            _bubbles.Value = Mathf.Clamp(value, 0f, maxLimit);
        }
    }

    public float maxValue => maxLimit;

    void Start()
    {
        _bubbles.OnValueChanged += UpdateUI;
    }

    [Rpc(SendTo.Server)]
    public void SetBubblesRPC(float value)
    {
        Bubbles = value;
    }

    public void UpdateUI(float prevValue, float internalValue)
    {
        if (bubbleCountText != null)
            bubbleCountText.text = internalValue.ToString();
    }
}
