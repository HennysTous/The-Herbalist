using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Items/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> allItems;

    private Dictionary<string, ItemData> lookup;

    public void Initialize()
    {
        lookup = new Dictionary<string, ItemData>();
        foreach (var item in allItems)
        {
            if (!lookup.ContainsKey(item.uniqueID))
                lookup.Add(item.uniqueID, item);
        }
    }

    public ItemData GetItemByID(string id)
    {
        if (lookup == null)
            Initialize();
        return lookup.ContainsKey(id) ? lookup[id] : null;
    }
}
