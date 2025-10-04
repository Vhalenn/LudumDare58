using UnityEngine;
using Core.Utilities;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private LevelList levelList;
    [SerializeField] private bool mainMenu;
    [SerializeField] private bool inMenu; public bool InMenu { get => inMenu; set => inMenu = value; }

    [Header("Elements")]
    public CanvasManager canvasManager;

    public void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void LoadLevel(int value)
    {
        Debug.Log("Load level");
        if(!levelList)
        {
            Debug.LogError("No level-list !!!");
            return;
        }

        SceneLoader.LoadScene(levelList.list[value].sceneName);
    }

    public void OnEscape()
    {
        Debug.Log("Pressed Escape");
        if (mainMenu) return;

        if(canvasManager)
        {
            canvasManager.PauseMenuChangeState();
        }
    }
}
