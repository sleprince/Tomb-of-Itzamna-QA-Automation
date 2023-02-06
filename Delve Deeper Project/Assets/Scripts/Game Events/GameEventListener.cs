using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityGameObjectEvent : UnityEvent<GameObject> { }

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityGameObjectEvent Response = new UnityGameObjectEvent();

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(GameObject obj)
    {
        Response.Invoke(obj);
    }
}
