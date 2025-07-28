using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity;

    public InventorySlot(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public void Add (int amount)
    {
        quantity += amount;
    }

    public void Remove(int amount)
    {
        quantity = Mathf.Max(0, quantity - amount);
    }
}
