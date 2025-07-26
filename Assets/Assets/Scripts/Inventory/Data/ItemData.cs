using UnityEngine;

public enum ItemType
{
    Plant,
    Consumable
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Identification")]
    public string uniqueID;              // Save/Load var
    public string itemName;              // Common name
    public string scientificName;        // Scientific Name
    public ItemType itemType;

    [Header("Visual")]
    public Sprite icon;
    public bool isGolden;                //Rare or Not

    [Header("Economy")]
    public int sellValue;

    [Header("Consumable (only if its ItemType = Consumable")]
    public SpeedBuffData speedBuff;                // Null

    [TextArea]
    public string description;
}
