using UnityEngine;

public class anim_pos : MonoBehaviour
{
    [SerializeField][Range(0.01f,1f)] float speed;
    [SerializeField] AnimationCurve curve;
    [SerializeField] Vector3 dir = Vector3.up;
    Vector3 localPos, fPos;

    void Start() {
        localPos = transform.localPosition;
    }

    void Update()
    {
        fPos = localPos;
        fPos += (dir * curve.Evaluate(Time.realtimeSinceStartup * speed) );
        transform.localPosition = fPos;
    }
}
