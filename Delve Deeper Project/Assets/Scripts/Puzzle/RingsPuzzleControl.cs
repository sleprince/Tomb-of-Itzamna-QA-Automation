using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class RingsPuzzleControl : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    bool interactHeld = false;

    RingsPuzzle puzzle;
    ThirdPersonController player;

    private void Awake()
    {
        player = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if (RingsPuzzle.RingsPuzzleCompleted)
            return;

        interactHeld = interactAction.action.ReadValue<float>() > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        puzzle = FindObjectOfType<RingsPuzzle>();

        if (RingsPuzzle.RingsPuzzleCompleted)
            return;
    }

    private void OnTriggerStay(Collider other)
    {
        RingPuzzleTrigger pt = null;

        if (RingsPuzzle.RingsPuzzleCompleted)
            return;

        if (other.GetComponent<RingPuzzleTrigger>())
        {
            pt = other.GetComponent<RingPuzzleTrigger>();
        }

        if (interactHeld)
        {
            if (pt != null)
            {
                puzzle.RotatePillar(pt);
                player.HandlePushing(true);
            }
        }
        else
        {
            player.HandlePushing(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (RingsPuzzle.RingsPuzzleCompleted)
            return;

        RingPuzzleTrigger pt = other.GetComponent<RingPuzzleTrigger>();
        pt = null;
    }
}
