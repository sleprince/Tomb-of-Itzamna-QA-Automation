using UnityEngine;

[CreateAssetMenu(menuName = "Item Data", fileName = "New Item", order = 51)]
public class ItemData : ScriptableObject
{
    [SerializeField] Texture icon;
    [SerializeField] string itemName;
    [TextArea]
    [SerializeField] string description;

    public Texture Icon { get { return icon; } }
    public string Name { get { return itemName; } }
    public string Description { get { return description; } }
}
