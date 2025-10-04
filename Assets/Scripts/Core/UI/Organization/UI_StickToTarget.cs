using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StickToTarget : MonoBehaviour
{
    Camera mainCam;
    public Transform target;
    private RectTransform trans;
    [SerializeField] UI_CanvasScale canvasScale;

    public float transitionSpeed = 0.5f;

    public enum VerPlace { Top, Center, Bottom };
    public enum HorPlace { Left, Center, Right };

    public VerPlace vPlace = VerPlace.Center;
    public HorPlace hPlace = HorPlace.Center;
    public Vector2 margin;

    void Start()
    {
        canvasScale = GetComponentInParent<UI_CanvasScale>();
        trans = GetComponent<RectTransform>();
        //mainCam = GetComponentInParent<scriptRef>().mainCam;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector2 ViewportPosition = mainCam.WorldToScreenPoint(target.position);
        ViewportPosition.x = Mathf.CeilToInt(ViewportPosition.x);
        ViewportPosition.y = Mathf.CeilToInt(ViewportPosition.y);

        Vector2 offset = Vector2.zero;

        if(vPlace == VerPlace.Top)
        {
            offset.y = trans.sizeDelta.y * 0.5f;
        }
        else if (vPlace == VerPlace.Bottom)
        {
            offset.y = -trans.sizeDelta.y * 0.5f;
        }

        if (hPlace == HorPlace.Left)
        {
            offset.x = -trans.sizeDelta.x * 0.5f;
        }
        else if (hPlace == HorPlace.Right)
        {
            offset.x = trans.sizeDelta.x * 0.5f;
        }


        offset /= canvasScale.ScreenScale; // FIT SCREEN
        Vector2 newPos = ViewportPosition + (offset * margin);
        trans.position = Vector2.Lerp(trans.position, newPos, transitionSpeed);
    }
}
