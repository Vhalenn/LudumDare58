using UnityEngine;

public class SceneMenuSelector : MonoBehaviour
{
    public LevelList levelList;

    void Start()
    {
        
    }

    public void LoadLevel(int index)
    {
        SceneLoader.LoadScene(index);
    }
}
