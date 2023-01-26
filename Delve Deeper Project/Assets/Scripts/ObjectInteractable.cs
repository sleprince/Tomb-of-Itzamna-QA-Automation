using UnityEngine;

public class ObjectInteractable : MonoBehaviour, IInteractable
{
    public void CollectObject()
    {
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return "Collect Item";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        CollectObject();
    }
}
