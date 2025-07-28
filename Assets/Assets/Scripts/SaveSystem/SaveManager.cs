using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("References")]
    public ItemDatabase itemDatabase; // Assign this in the Inspector

    private string savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        savePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        // Ensure item lookup is initialized
        itemDatabase.Initialize();
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        // Save current inventory slots
        foreach (var slot in InventorySystem.Instance.slots)
        {
            if (slot.item == null) continue;

            data.savedSlots.Add(new InventorySlotSave
            {
                itemID = slot.item.uniqueID,
                quantity = slot.quantity
            });
        }

        // Save current coins
        data.coins = CurrencySystem.Instance.Coins;

        // Save active buffs (speed multipliers)
        data.speedMultiplierBuffs = UpgradesManager.Instance.GetAppliedBuffs();

        // Save current inventory size (including expansions)
        data.currentInventorySize = InventorySystem.Instance.slots.Count;

        // Save NPC vendor upgrade level
        data.purchasedUpgrades = VendorUI.Instance.currentUpgradeLevel;

        // Write save file
        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
        Debug.Log("Game saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Save file not found.");
            return;
        }

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));

        // Clear current inventory
        InventorySystem.Instance.Clear();

        // Expand inventory up to the saved size
        while (InventorySystem.Instance.slots.Count < data.currentInventorySize)
        {
            InventorySystem.Instance.TryExpandInventory();
        }

        // Load saved items into inventory
        foreach (var slotSave in data.savedSlots)
        {
            var item = itemDatabase.GetItemByID(slotSave.itemID);
            if (item == null)
            {
                Debug.LogWarning($"Item with ID {slotSave.itemID} not found in the item database.");
                continue;
            }

            for (int i = 0; i < slotSave.quantity; i++)
                InventorySystem.Instance.AddItem(item);
        }

        // Restore coin count
        CurrencySystem.Instance.SetCoins(data.coins);

        // Apply saved buffs
        UpgradesManager.Instance.SetAppliedBuffs(data.speedMultiplierBuffs);

        // Restore vendor upgrade progress
        VendorUI.Instance.currentUpgradeLevel = data.purchasedUpgrades;

        // Refresh UI
        InventoryUIManager.Instance.RefreshInventory();
    }
}
