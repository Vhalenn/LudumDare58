using UnityEngine;

using TMPro;
using Febucci.UI;

public class UI_Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private TextAnimator_TMP textAnimator;

    [Header("Storage")]
    [SerializeField] private float actualPercentage;
    [SerializeField] private float timeLeft;
    [SerializeField] private float totalTime;
    [SerializeField] private string color;
    [SerializeField] private float scale;
    [SerializeField] private string finalText;

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
        
            this.timeLeft = timeLeft; // totalTime - (CoreUtility.CurrentTime - timeLeft);
            this.totalTime = totalTime;

            if (timeLeft > totalTime - 1f) // Two First seconds
            {
                finalText = "GO!";
            }
            else
            {
                actualPercentage = 1 - (timeLeft / totalTime);
                scale = scaleCurve.Evaluate(actualPercentage) * 100f;
                color = ColorUtility.ToHtmlStringRGB(colorGradient.Evaluate(actualPercentage));


                finalText = $"<color=#{color}><size={scale}%>{timeLeft:F1}</scale></color>";
                
                if (timeLeft < 5f)
                {
                    finalText = $"<shake>{finalText}</shake>";
                }
                
            }

        
        if (textAnimator)
        {
            textAnimator.SetText(finalText);
        }
        else if (timerText)
        {
            timerText.text = finalText;
        }
    }
}
