using UnityEngine;

public class randRot : MonoBehaviour
{
    [SerializeField] bool SquareAngle;
    [SerializeField] Vector2 range;

    private void OnEnable() {
        float rotY = 0;

        if(SquareAngle) rotY = Random.Range(0,4) * 90;
        else rotY = Random.Range(range.x,range.y);

        transform.localRotation = Quaternion.Euler(0, rotY, 0);
    }
}
