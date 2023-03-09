using UnityEngine;
using UnityEngine.Events;

public class DisplayOnTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask layers;
    [SerializeField] private UnityEvent OnEnter, OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            OnEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer))
        {
            OnExit.Invoke();
        }
    }
}
