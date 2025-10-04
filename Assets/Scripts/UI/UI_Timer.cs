using UnityEngine;

using TMPro;
public class UI_Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private AnimationCurve scaleCurve;

    [Header("Storage")]
    [SerializeField] private float actualPercentage;
    [SerializeField] private float timeLeft;
    [SerializeField] private string color;
    [SerializeField] private float scale;

    private void Start()
    {
        ClearTimer();
    }

    public void ClearTimer()
    {
        if(timerText)
        {
            timerText.text = string.Empty;
        }
    }

    public void SetTimer(float timeLeft, float totalTime)
    {
        if(timerText)
        {
            this.timeLeft = timeLeft; // totalTime - (CoreUtility.CurrentTime - timeLeft);

            if(timeLeft > totalTime - 1f) // Two First seconds
            {
                timerText.text = "GO!";
            }
            else
            {
                actualPercentage = totalTime / timeLeft;
                scale = scaleCurve.Evaluate(actualPercentage) * 100f;
                color = ColorUtility.ToHtmlStringRGB(colorGradient.Evaluate(actualPercentage));

                timerText.text = $"<color=#{color}><size={scale}%>{timeLeft:F1}</scale></color>";
                /*
                if (timeLeft < 5f)
                {
                    timerText.text = $"<color=red>{timeLeft:F1}</color>";
                }
                */
            }
        }
    }
}
