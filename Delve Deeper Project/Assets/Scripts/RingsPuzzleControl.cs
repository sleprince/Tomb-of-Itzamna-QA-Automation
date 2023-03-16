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
<<<<<<< Updated upstream:Delve Deeper Project/Assets/Scripts/RingsPuzzleControl.cs
        puzzle = FindObjectOfType<RingsPuzzle>();
        player = FindObjectOfType<ThirdPersonController>();
=======
        player = GetComponent<ThirdPersonController>();
>>>>>>> Stashed changes:Delve Deeper Project/Assets/Scripts/Puzzle/RingsPuzzleControl.cs
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
<<<<<<< Updated upstream:Delve Deeper Project/Assets/Scripts/RingsPuzzleControl.cs

=======
                //player.HandlePushing(true);
>>>>>>> Stashed changes:Delve Deeper Project/Assets/Scripts/Puzzle/RingsPuzzleControl.cs
            }
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
