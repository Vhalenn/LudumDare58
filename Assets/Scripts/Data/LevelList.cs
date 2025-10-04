using UnityEngine;

[CreateAssetMenu]
public class LevelList : ScriptableObject
{
    public Level[] list;
}

[System.Serializable]
public struct Level
{
    public string name;
    public string sceneName;
    public Sprite sprite;
    public int difficulty;
}
