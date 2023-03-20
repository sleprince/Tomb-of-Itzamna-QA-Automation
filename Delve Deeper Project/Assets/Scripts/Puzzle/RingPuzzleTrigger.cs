using UnityEngine;

public enum RingPuzzleTriggerType
{
    OUTER = 0,
    MIDDLE,
    INNER
}

public enum RingPuzzleTriggerDirection
{
    Clockwise = 0,
    AntiClockwise
}

public class RingPuzzleTrigger : MonoBehaviour, IInteractable
{
    public RingPuzzleTriggerType type;
    public RingPuzzleTriggerDirection direction;

    public string GetInteractText()
    {
        return "(HOLD) Rotate Pillar";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        
    }
}
