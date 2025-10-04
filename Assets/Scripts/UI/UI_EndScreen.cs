using UnityEngine;

using Sirenix.OdinInspector;

public class UI_EndScreen : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;

    [Header("Elements")]
    [SerializeField] private GameObject winObject;
    [SerializeField] private GameObject looseObject;

    [Header("Storage")]
    [SerializeField] private bool init;

    private void Start()
    {
        if (init) return;

        Hide();
    }

    [Button] public void ShowWinScreen() => ShowState(win: true);
    [Button] public void ShowLooseScreen() => ShowState(win: false);

    private void ShowState(bool win)
    {
        if (Application.isPlaying)
        {
            init = true;

            if(canvasManager)
            {
                canvasManager.game.InMenu = true;
            }
        }

        gameObject.SetActive(true);

        winObject.SetActive(win);
        looseObject.SetActive(!win);
    }

    [Button]
    public void Hide()
    {
        if (canvasManager)
        {
            canvasManager.game.InMenu = false;
        }

        gameObject.SetActive(false);
    }
}
