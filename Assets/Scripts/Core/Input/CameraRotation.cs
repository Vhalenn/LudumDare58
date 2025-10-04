using UnityEngine;

using Sirenix.OdinInspector;

public class CameraRotation : MonoBehaviour
{
    [ReadOnly] public Vector2 controlInput;
    [SerializeField] private float rotSpeed = 1f;


    [Header("Storage")]
    [SerializeField] private Vector3 localRot;

    private void Start()
    {
        
    }

    private void Update()
    {

        if (GameManager.instance && GameManager.instance.InMenu)
        {
            Cursor.lockState = CursorLockMode.Confined;

            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        localRot = transform.eulerAngles;

        localRot.y += controlInput.x * rotSpeed;

        transform.localEulerAngles = localRot;
    }
}
