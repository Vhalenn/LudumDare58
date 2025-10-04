using UnityEngine;

public class DemonDen : MonoBehaviour
{
    [SerializeField] private WorldManager world;

    [Header("Elements")]
    [SerializeField] private SphereCollider selfCollider;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (!selfCollider) return;

        CoreUtility.DrawCircle(Color.red, transform.position + selfCollider.center, selfCollider.radius);
    }
}
