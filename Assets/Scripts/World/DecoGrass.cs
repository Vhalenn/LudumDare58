using UnityEngine;

using Sirenix.OdinInspector;
public class DecoGrass : MonoBehaviour
{

    [Header("Raycast")]
    [SerializeField] private bool drawGizmo = true;
    [SerializeField] private float radius = 4f;
    [SerializeField] private float density = 4f;

    [Header("Storage")]
    [SerializeField] private Matrix4x4[] grassMatrix;
    public Matrix4x4[] GrassMatrix => grassMatrix;

    // Update is called once per frame
    void Update()
    {
        
    }

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

        CoreUtility.DrawCircle(Color.green, transform.position, radius);
        Gizmos.color = Color.green;

        int length = grassMatrix.Length;
        for (int i = 0; i < length; i++)
        {
            Matrix4x4 m = grassMatrix[i];
            CoreUtility.DrawCircle(Color.green, m.GetPosition(), m.lossyScale.magnitude * 0.5f);

            //Gizmos.DrawSphere(m.GetPosition(), m.lossyScale.magnitude * 0.5f);
        }
    }
}
