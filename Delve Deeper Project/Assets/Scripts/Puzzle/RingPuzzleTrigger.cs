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

{
    public RingPuzzleTriggerType type;
    public RingPuzzleTriggerDirection direction;

    public string GetInteractText()
    {
        return "Rotate Pillar";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        
    }
}
