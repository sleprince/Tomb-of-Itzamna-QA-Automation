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

public class RingPuzzleTrigger : MonoBehaviour
{
    public RingPuzzleTriggerType type;
    public RingPuzzleTriggerDirection direction;
}
