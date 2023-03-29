using UnityEngine;
using UnityEngine.Events;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private LayerMask layers;
    [SerializeField] private UnityEvent OnEnter, OnExit;
    [SerializeField] private bool GotKey = false;
    private bool isOpen = false;

    public void OnKeyCollected()
    {
        GotKey = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer) && GotKey)
        {
            OnEnter.Invoke();
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (0 != (layers.value & 1 << other.gameObject.layer) && isOpen)
        {
            OnExit.Invoke();
        }
    }
}
