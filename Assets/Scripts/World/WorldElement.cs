using UnityEngine;

public class WorldElement : MonoBehaviour
{
    [SerializeField] protected WorldManager world; public WorldManager World { get => world; set => world = value; }
    [SerializeField] private string publicName; public string PublicName => publicName;

    private void Start()
    {
        name += PublicName;
    }
}
