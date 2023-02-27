using UnityEngine;

public enum PuzzleTriggerType
{
    OUTER = 0,
    MIDDLE,
    INNER
}

public enum PuzzleTriggerDirection
{
    Clockwise = 0,
    AntiClockwise
}

public class PuzzleTrigger : MonoBehaviour
{
    public PuzzleTriggerType type;
    public PuzzleTriggerDirection direction;
}
