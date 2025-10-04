using UnityEngine;


public class RotateObject : MonoBehaviour
{
    [SerializeField] Vector3 dir = Vector3.up;
    [SerializeField] float speed = 15.0f;

    void Update()
    {
        transform.Rotate(dir * Time.deltaTime * speed, Space.World);
    }
}
