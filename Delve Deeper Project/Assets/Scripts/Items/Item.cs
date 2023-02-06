using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameEvent itemCollected;
    [SerializeField] ItemData info;
    SetItemInfo setItemInfo;

    private void Start()
    {
        //setItemInfo = GameObject.FindWithTag("BookUI").GetComponent<SetItemInfo>();
    }

    public void SetInfo()
    {
        setItemInfo.itemIcon.GetComponent<RawImage>().texture = info.Icon;
        setItemInfo.itemName.text = info.Name;
        setItemInfo.itemDesc.text = info.Description;
    }
}
