using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    [SerializeField] private int maxSlots = 6;
    public int MaxLimit = 18;
    public int ExpansionSize = 4;

    private List<InventorySlot> slots = new();
    public IReadOnlyList<InventorySlot> Slots => slots;

    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        FillEmptySlots();
        OnInventoryChanged?.Invoke();
    }

    private void FillEmptySlots()
    {
        while (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot(null, 0));
        }
    }

    public bool AddItem(ItemData item)
    {
        // First, It'll try to stack equal items into a single slot per item
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.Add(1);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        // Then, try a empty slot
        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.quantity = 1;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        Debug.Log("Inventory Full.");
        return false;
    }

    public void RemoveItem(ItemData item)
    {
        var slot = slots.Find(s => s.item == item);
        if (slot != null)
        {
            slot.Remove(1);
            if (slot.quantity <= 0)
            {
                slot.item = null;
                slot.quantity = 0;
            }
            OnInventoryChanged?.Invoke();
        }
    }

    /*public void SwapSlots(int indexA, int indexB)
    {
        if (indexA >= slots.Count || indexB >= slots.Count) return;

        var temp = slots[indexA];
        slots[indexA] = slots[indexB];
        slots[indexB] = temp;

        OnInventoryChanged?.Invoke();
    }*/

    public void SwapSlots(int indexA, int indexB)
    {
        if (indexA >= slots.Count || indexB >= slots.Count) return;

        // Swap data, NOT object reference
        var slotA = slots[indexA];
        var slotB = slots[indexB];

        ItemData tempItem = slotA.item;
        int tempQty = slotA.quantity;

        slotA.item = slotB.item;
        slotA.quantity = slotB.quantity;

        slotB.item = tempItem;
        slotB.quantity = tempQty;

        OnInventoryChanged?.Invoke();
    }

    public void Clear()
    {
        slots.Clear();
        FillEmptySlots();
        OnInventoryChanged?.Invoke();
    }

    public bool TryExpandInventory()
    {
        if (maxSlots >= MaxLimit) return false;

        maxSlots = Mathf.Min(maxSlots + ExpansionSize, MaxLimit);
        FillEmptySlots();
        OnInventoryChanged?.Invoke();
        return true;
    }
}