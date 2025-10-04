using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameManager game;

    [Header("Elements")]
    public Canvas mainCanvas;
    public Canvas pauseCanvas;
    public Canvas settingsCanvas;
    public Canvas levelSelectionCanvas;

    [SerializeField] GameObject test;

    private void Start()
    {
        if (pauseCanvas)
        {
            pauseCanvas.enabled = false;
        }
    }

    public void Play() // Load scene if not in menu
    {
        GameManager.instance.LoadLevel(value:0);
    }

    public void PauseMenuChangeState()
    {
        if(pauseCanvas)
        {
            pauseCanvas.enabled = !pauseCanvas.enabled;
        }

        if(game)
        {
            game.InMenu = pauseCanvas.enabled;
        }
    }

    public void Options()
    {
        
    }
}
