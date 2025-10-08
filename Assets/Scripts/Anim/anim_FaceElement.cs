using UnityEngine;

using Sirenix.OdinInspector;
public class anim_FaceElement : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Material mat;

    [Header("Stats")]
    [SerializeField] private float arraySpeed = 1;

    [Header("Offset")]
    [SerializeField] private Vector2[] offsetArray;

    [Header("Storage")]
    [SerializeField] private int length;
    [SerializeField] private float currentTime;
    [SerializeField] private Vector2 offset;

    [Button]
    public void Init()
    {
        if (mesh) mat = mesh.sharedMaterial;

        length = offsetArray.Length;
    }

    private float ValueFromTime(float time)
    {
        return (Mathf.Sin(time * 4.15f) + Mathf.Sin(time * 2.25f) + 2f) * 0.25f;
    }

    private void Update()
    {
        currentTime = CoreUtility.CurrentTime;
        Refresh();
    }

    private void Refresh()
    {
        if (!mat) return;

        int index = Mathf.FloorToInt(ValueFromTime(currentTime * arraySpeed) * length);
        offset = offsetArray[index];

        mat.mainTextureOffset = offset;
    }
}
