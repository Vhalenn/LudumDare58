using UnityEngine;

using Sirenix.OdinInspector;
public class DecoGrass : MonoBehaviour
{

    [Header("Raycast")]
    [SerializeField] private bool drawGizmo = true;
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] private float radius = 4f;
    [SerializeField] private float density = 4f;

    [Header("Storage")]
    [SerializeField] private Matrix4x4[] grassMatrix;
    public Matrix4x4[] GrassMatrix => grassMatrix;

    public bool GizmoState => drawGizmo;
    public void SetGizmoState(bool state) => drawGizmo = state;

    [Button]
    private void SamplePositions()
    {
        int totalProps = Mathf.FloorToInt(radius * density);

        grassMatrix = new Matrix4x4[totalProps];

        for (int i = 0; i < totalProps; i++)
        {
            Vector2 pos = Random.insideUnitCircle * radius;
            Vector3 finalPos = transform.position + new Vector3(pos.x, 0, pos.y);

            Matrix4x4 m = new();

            // Rot
            Quaternion q = Quaternion.Euler(0.1f, Random.Range(-180f, 180f), 0.1f);

            // Scale
            Vector3 scale = Vector3.one * Random.Range(0.8f, 1.2f);
            // Apply

            m.SetTRS(finalPos, q, scale);
            grassMatrix[i] = m;
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        CoreUtility.DrawCircle(gizmoColor * 0.4f, transform.position, radius);

        int length = grassMatrix.Length;
        for (int i = 0; i < length; i++)
        {
            Matrix4x4 m = grassMatrix[i];
            CoreUtility.DrawCircle(gizmoColor, m.GetPosition(), m.lossyScale.magnitude * 0.25f);
        }
    }
}
