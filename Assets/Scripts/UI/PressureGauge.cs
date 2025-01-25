using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PressureGauge : MonoBehaviour
{

    public Image bubbleGauge_WarningLight;
    public Image bubbleGauge_BubbleLevel;

    public TextMeshProUGUI bubbleGauge_BubbleLevelText;
    public TextMeshProUGUI digitalTimer_TimerText;

    public int currentBubbleCount;
    public int maxBubbleCount = 25;

    public float warningLightActivationPercentage = 0.45f;

    private float bubblePercentage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBubbleCount = maxBubbleCount;
    }

    // Update is called once per frame
    void Update()
    {
        bubblePercentage = (float)currentBubbleCount / maxBubbleCount;

        HandleBubbleLevel();
        HandleWarningLight();
    }

    void HandleBubbleLevel()
    {
        if (bubbleGauge_BubbleLevel == null || bubbleGauge_BubbleLevelText == null) return;

        bubbleGauge_BubbleLevel.fillAmount = bubblePercentage;
        bubbleGauge_BubbleLevelText.text = currentBubbleCount.ToString();
    }

    void HandleWarningLight()
    {
        if (bubbleGauge_WarningLight == null) return;

        if (bubblePercentage <= warningLightActivationPercentage) 
        {
            Color lightColour = bubbleGauge_WarningLight.color;
            lightColour.a = Mathf.Lerp(0.2f, 1f, Mathf.PingPong(Time.time, 1));
            bubbleGauge_WarningLight.color = lightColour;
        }
        else 
        {
            Color lightColour = bubbleGauge_WarningLight.color;
            lightColour.a = 0;
            bubbleGauge_WarningLight.color = lightColour;
        }
        
    }
}
