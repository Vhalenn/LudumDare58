using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class GraphicButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    public Image background;
    public TextMeshProUGUI text;

    [SerializeField]
    private Material normal, highlight;

    [Header("SceneToLoad")]
    public int SceneIndex;

    [Header("Debug")][SerializeField]
    private RectTransform rectTransform;
    public Vector2 size;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(transform.name + " clicked on");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOScale(Vector3.one * 1.1f, 0.2f);
        if (background != null) background.material = highlight;
        //rectTransform.DOLocalMoveZ(-75, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOScale(Vector3.one, 0.2f);
        if (background != null) background.material = normal;
        //rectTransform.DOLocalMoveZ(0, 0.1f);
    }
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        size = rectTransform.sizeDelta;
    }
}
