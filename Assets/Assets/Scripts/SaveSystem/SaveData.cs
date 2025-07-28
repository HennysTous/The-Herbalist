using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<InventorySlotSave> savedSlots = new();
    public int coins;
    public List<ItemData> speedMultiplierBuffs;
    public int currentInventorySize;
    public int purchasedUpgrades;
}

[System.Serializable]
public class InventorySlotSave
{
    public string itemID;
    public int quantity;
}
