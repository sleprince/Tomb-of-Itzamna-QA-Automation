using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragObjectControl : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;
    bool interactHeld = false;

    MovableObject movableObject;
    ThirdPersonController player;
    [SerializeField] private GameObject torch;

    private void Awake()
    {
        player = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        interactHeld = interactAction.action.ReadValue<float>() > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        movableObject = FindObjectOfType<MovableObject>();
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerInteractable trig = null;

        if (other.GetComponent<TriggerInteractable>())
        {
            trig = other.GetComponent<TriggerInteractable>();
        }

        if (interactHeld)
        {
            torch.SetActive(false);

            if (trig != null)
            {
                movableObject.MoveObject(trig);
                player.HandlePulling(true);
            }
        }
        else
        {
            torch.SetActive(true);
            player.HandlePulling(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RingPuzzleTrigger pt = other.GetComponent<RingPuzzleTrigger>();
        pt = null;
    }
}
