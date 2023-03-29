using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerUIObject;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private PlayerInteract playerInteract;

    private void Update()
    {
        if (playerInteract.m_Interactable != null)
        {
            Show(playerInteract.m_Interactable);
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
