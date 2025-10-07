using UnityEngine;
using UnityEngine.Splines;

public class WorldRoad : WorldElement
{
    [Header("Elements")]
    [SerializeField] private SplineContainer spline;

    [Header("stats")]
    [SerializeField] private float roadwidth = 2;
    [SerializeField] private float roadLength = 10;

    private void OnDrawGizmos()
    {
        if(spline)
        {
            float length = spline.CalculateLength();
            float gap = 1.0f / length;

            for (int i = 0; i < length; i++)
            {
                float splinePos = gap * i;
                Vector3 pos = spline.EvaluatePosition(splinePos);
                Vector3 posB = spline.EvaluatePosition(gap * (i+1));

                Vector3 tangent = spline.EvaluateTangent(splinePos);
                tangent = tangent.normalized * roadwidth;
                tangent = Quaternion.AngleAxis(90, Vector3.up) * tangent;

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(pos + tangent, posB + tangent);
                Gizmos.DrawLine(pos - tangent, posB - tangent);
                //Gizmos.DrawLine(basePos + dir - width, basePos - dir - width);
            }

            return;
        }

        // IF no spline
        Vector3 dir = transform.forward * roadLength * 0.5f;
        Vector3 width = transform.right * roadwidth * 0.5f;
        Vector3 basePos = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(basePos + dir + width, basePos - dir + width);
        Gizmos.DrawLine(basePos + dir - width, basePos - dir - width);
    }
}
