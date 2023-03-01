using UnityEngine;

public class KeyItemInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        Collect();
    }

    private void Collect()
    {
        KeyItem item = GetComponent<KeyItem>();
        item.Event.Raise(this.gameObject);
        Destroy(gameObject);
    }
}
