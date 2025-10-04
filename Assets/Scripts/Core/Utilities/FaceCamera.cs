using UnityEngine;

using Sirenix.OdinInspector;
using Unity.Cinemachine;
using TMPro;

public class FaceCamera : MonoBehaviour
{
    [Header("Self")]
    [SerializeField] private Transform _tr;

    [Header("Camera Storage")]
    [SerializeField, Tooltip("Will be auto-assigned")] private Camera cam;
    [SerializeField] private Vector3 rot;

    [Header("Settings")]
    [SerializeField] private bool rotateOnlyY;
    [SerializeField, EnableIf("rotateOnlyY")] private float verticalRotation = 90;

    [Button]
    private void GetCam()
    {
        if (cam) return;

        cam = Camera.main; // Costly but better than nothing
    }

    private void LateUpdate()
    {
        AlignToCamera();
    }

    [Button]
    private void AlignToCamera()
    {
        if (!_tr || !_tr.gameObject.activeInHierarchy) return;

        if (!cam) GetCam();

        if (cam)
        {
            rot = cam.transform.rotation.eulerAngles;
            if (rotateOnlyY) rot = new Vector3(verticalRotation, rot.y, 0);
            _tr.rotation = Quaternion.Euler(rot);
        }
    }
}

