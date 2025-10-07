using UnityEngine;

public class TriggerSafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if(other.TryGetComponent(out Player pj))
        {
            pj.AddTriggerZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent(out Player pj))
        {
            pj.RemoveTriggerZone(this);
        }
    }
}
