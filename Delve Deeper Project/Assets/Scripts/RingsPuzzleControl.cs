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
        puzzle = FindObjectOfType<RingsPuzzle>();
        player = FindObjectOfType<ThirdPersonController>();
    }

    private void Update()
    {
        if (RingsPuzzle.RingsPuzzleCompleted)
            return;

        interactHeld = interactAction.action.ReadValue<float>() > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (RingsPuzzle.RingsPuzzleCompleted)
            return;
    }

    private void OnTriggerStay(Collider other)
    {
        PuzzleTrigger pt = null;

        if (RingsPuzzle.RingsPuzzleCompleted)
            return;

        if (other.GetComponent<PuzzleTrigger>())
        {
            pt = other.GetComponent<PuzzleTrigger>();
        }

        if (interactHeld)
        {
            if (pt != null)
            {
                puzzle.RotatePillar(pt);

            }
        }
    }
}
