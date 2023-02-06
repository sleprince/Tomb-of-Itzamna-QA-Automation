using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerUIObject;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private PlayerInteract playerInteract;

    private void Update()
    {
        if (playerInteract.GetInteractable() != null)
        {
            Show(playerInteract.GetInteractable());
        }
        else
        {
            Hide();
        }
    }

    void Show(IInteractable interactable)
    {
        containerUIObject.SetActive(true);
        interactText.text = interactable.GetInteractText();
    }

    void Hide()
    {
        containerUIObject.SetActive(false);
    }
}
