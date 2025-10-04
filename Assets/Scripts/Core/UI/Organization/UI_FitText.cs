using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_FitText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txt;
    public bool overflow;
    [SerializeField]
    private RectTransform BG;

    public Vector2 size;

    void Update()
    {
        size = new Vector2(txt.renderedWidth, txt.renderedHeight);
        //BG.sizeDelta = size + new Vector2(15,15);
        overflow = txt.isTextTruncated;

        if (overflow) GetComponent<RectTransform>().sizeDelta += Vector2.up;
    }
}
