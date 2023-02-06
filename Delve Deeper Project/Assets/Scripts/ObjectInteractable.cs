using UnityEngine;

public class ObjectInteractable : MonoBehaviour, IInteractable
{
    public void CollectObject()
    {
        Item item = GetComponent<Item>();
        item.itemCollected.Raise(this.gameObject);
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
