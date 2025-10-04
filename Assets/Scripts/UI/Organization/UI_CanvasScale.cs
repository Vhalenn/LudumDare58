using UnityEngine;
using UnityEngine.UI;

public class UI_CanvasScale : MonoBehaviour
{
    private CanvasScaler canvasScaler;
    public Vector2 ScreenScale
    {
        get
        {
            if (canvasScaler == null) {
                canvasScaler = GetComponent<CanvasScaler>();
            }

            if (canvasScaler) {
                return new Vector2(canvasScaler.referenceResolution.x / Screen.width, canvasScaler.referenceResolution.y / Screen.height);
            }
            else {
                return Vector2.one;
            }
        }
    }
}
