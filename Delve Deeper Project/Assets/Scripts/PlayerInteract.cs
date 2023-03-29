using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    public IInteractable m_Interactable;

    void Update()
    {
        if (interactAction.action.triggered)
        {
            if (m_Interactable != null)
            {
                m_Interactable.Interact(transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            m_Interactable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_Interactable = null;
    }
}
