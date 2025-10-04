using UnityEngine;

public class WorldWaypoint : MonoBehaviour
{
    [SerializeField] protected WorldManager world;


    [Header("Elements")]
    [SerializeField] private WorldCharacter[] characterArray;

    [Header("Stats")]
    [SerializeField] private float size;

    [Header("Storage")]
    [SerializeField] private float corruptionLevel; // How much the demon has taken here


    private void OnDrawGizmos()
    {
        CoreUtility.DrawCircle(Color.green, transform.position + Vector3.up * 0.1f, size);
    }
}
