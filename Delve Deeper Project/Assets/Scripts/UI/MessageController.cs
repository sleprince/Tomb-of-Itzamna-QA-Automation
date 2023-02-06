using UnityEngine;
using TMPro;

public class MessageController : MonoBehaviour
{
    TextMeshProUGUI messageText;

    private void Start()
    {
        messageText = this.GetComponent<TextMeshProUGUI>();
        messageText.enabled = false;
    }

    public void SetMessage(GameObject obj)
    {
        messageText.enabled = true;
        messageText.text = "Journal updated";
        Invoke("DisableAfterTime", 2f);
    }

    void DisableAfterTime()
    {
        messageText.enabled = false;
    }
}
