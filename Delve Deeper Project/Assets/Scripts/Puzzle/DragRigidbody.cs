using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class DragRigidbody : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private float forceAmount = 500;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private ThirdPersonController playerMovement;
    [SerializeField] private GameObject torch;

    Rigidbody selectedRigidbody;
    Camera targetCam;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPosition;
    float selectionDistance;
    CharacterController playerController;

    private void Start()
    {
        targetCam = GetComponent<Camera>();
        playerController = FindObjectOfType<CharacterController>();
    }

    private void Update()
    {
        if (!targetCam)
            return;

        ButtonControl control = interactAction.action.controls[0] as ButtonControl;
        if (control.wasPressedThisFrame)
        {
            selectedRigidbody = GetRigidbodyFromScreenCentre();
            playerMovement.LockCameraPosition = true;
            torch.SetActive(false);
        }

        if (control.wasReleasedThisFrame)
        {
            playerMovement.LockCameraPosition = false;
            playerMovement.HandlePulling(false);       
            torch.SetActive(true);

            selectedRigidbody = null;
        }
    }

    private void FixedUpdate()
    {
        if (selectedRigidbody)
        {
            Vector3 positionOffset = targetCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, selectionDistance)) - originalScreenTargetPosition;
            selectedRigidbody.velocity = (originalRigidbodyPosition + positionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;

            playerController.transform.forward = (new Vector3(selectedRigidbody.transform.position.x, playerController.transform.position.y,
                                                        selectedRigidbody.transform.position.z) - playerController.transform.position).normalized;

            playerMovement.HandlePulling(true);
        }
    }

    Rigidbody GetRigidbodyFromScreenCentre()
    {
        Ray ray = targetCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, interactLayer))
        {
            if (hit.collider.gameObject.GetComponent<Rigidbody>())
            {
                selectionDistance = Vector3.Distance(ray.origin, hit.point);
                originalScreenTargetPosition = targetCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, selectionDistance));
                originalRigidbodyPosition = hit.collider.transform.position;
                return hit.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }
}
