using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetItemInfo : MonoBehaviour
{
    public GameObject bookUiPanel;
    public GameObject itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;

    public void OpenBook()
    {
        bookUiPanel.SetActive(true);
    }

    public void CloseBook()
    {
        bookUiPanel.SetActive(false);
    }
}
