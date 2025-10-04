using UnityEngine;

public class WorldRoad : WorldElement
{
    [Header("stats")]
    [SerializeField] private float roadwidth = 2;
    [SerializeField] private float roadLength = 10;

    private void OnDrawGizmos()
    {
        Vector3 dir = transform.forward * roadLength * 0.5f;
        Vector3 width = transform.right * roadwidth * 0.5f;
        Vector3 basePos = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(basePos + dir + width, basePos - dir + width);
        Gizmos.DrawLine(basePos + dir - width, basePos - dir - width);
    }
}
