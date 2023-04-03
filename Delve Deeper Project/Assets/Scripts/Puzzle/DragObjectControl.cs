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
            if (trig != null)
            {
                player.LockCameraPosition = !player.LockCameraPosition;
                player.MoveSpeed = 0f;
                torch.SetActive(false);
                movableObject.MoveObject(trig);
                player.HandlePulling(true);
                player.MovingHeavyObject = true;
            }
        }
        else
        {
            player.MoveSpeed = 2f;
            torch.SetActive(true);
            player.HandlePulling(false);
            player.MovingHeavyObject = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerInteractable trig = other.GetComponent<TriggerInteractable>();
        trig = null;
    }
}
