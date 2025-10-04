using UnityEngine;
using UnityEngine.Events;

public class ActionOnTrigger : MonoBehaviour
{
    [SerializeField] private bool executeExitFunctionAtStart;
    [SerializeField] private UnityEvent OnEnter;
    [SerializeField] private UnityEvent OnExit;

    private void Start()
    {
        if(executeExitFunctionAtStart)
        {
            TriggerExit();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        TriggerEnter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        TriggerExit();
    }

    private void TriggerEnter()
    {
        //Debug.LogError("TRIGGER -> Enter");

        OnEnter?.Invoke();
    }

    private void TriggerExit()
    {
        //Debug.LogError("TRIGGER -> Exit");

        OnExit?.Invoke();
    }
}
