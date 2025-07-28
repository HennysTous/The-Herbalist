using UnityEngine;

public enum ItemType
{
    Plant,
    Consumable
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    //Basic Data
    public string uniqueID;              // Save/Load var
    public string itemName;              // Common name
    public string scientificName;        // Scientific Name
    public ItemType itemType;

    public Sprite icon;

    public int sellValue;


    // Only if its ItemType = Consumable"
    public float speedMultiplier;
    public string description;
}
