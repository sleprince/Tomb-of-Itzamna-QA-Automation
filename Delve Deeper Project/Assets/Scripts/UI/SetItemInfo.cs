using TMPro;
using UnityEngine;

public class SetItemInfo : MonoBehaviour
{
    public GameObject journalUIPanel;
    public GameObject itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;

    public void OpenJournal()
    {
        journalUIPanel.SetActive(true);
    }

    public void CloseJournal()
    {
        journalUIPanel.SetActive(false);
    }
}
