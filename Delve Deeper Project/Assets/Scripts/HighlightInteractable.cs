using UnityEngine;

public class HighlightInteractable : MonoBehaviour
{
    private RaycastHit hit;
    [SerializeField] private Transform playerCamTransform;
    [SerializeField] private float hitRange = 3f;
    [SerializeField] private LayerMask interactMask;

    private void FixedUpdate()
    {
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.Toggle(false);
        }
        if (Physics.Raycast(playerCamTransform.position, playerCamTransform.forward, out hit, hitRange, interactMask))
        {
            hit.collider.GetComponent<Highlight>()?.Toggle(true);
        }
    }
}
