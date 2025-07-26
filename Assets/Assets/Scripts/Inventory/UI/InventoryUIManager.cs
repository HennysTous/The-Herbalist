using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private TooltipUI tooltipPanel;
    [SerializeField] private ContextMenuUI contextMenu;

    private List<InventorySlotUI> uiSlots = new();
    private InventorySlotUI draggingSlot;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        InventorySystem.Instance.OnInventoryChanged += RefreshInventory;
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        foreach (var slot in uiSlots)
            Destroy(slot.gameObject);
        uiSlots.Clear();

        var inventory = InventorySystem.Instance.Slots;

        for (int i = 0; i < inventory.Count; i++)
        {
            var uiSlot = Instantiate(slotPrefab, slotParent).GetComponent<InventorySlotUI>();
            uiSlot.SetupSlot(inventory[i], i);
            uiSlots.Add(uiSlot);
        }
    }

    public void ShowTooltip(InventorySlot slot)
    {
        if (slot == null || slot.item == null)
        {
            Debug.LogWarning("There's no item to show.");
            return;
        }

        tooltipPanel.Show(slot.item);
    }

    public void HideTooltip() => tooltipPanel.Hide();

    public void ShowContextMenu(InventorySlot slot, Vector3 position, InventorySlotUI uiRef)
    {
        if (slot?.item == null) return;
        contextMenu.Show(slot, position, uiRef);
    }

    public void HideContextMenu() => contextMenu.Hide();

    // ---------- DRAG & DROP -----------

    public void BeginDrag(InventorySlotUI slot)
    {
        draggingSlot = slot;
        slot.iconImage.raycastTarget = false;
    }

    public void EndDrag(InventorySlotUI fromSlot)
    {
        InventorySlotUI toSlot = GetSlotUnderMouse();

        if (toSlot != null && toSlot != fromSlot)
        {
            Debug.Log($"Ended Drag: From {fromSlot.slotIndex} to {toSlot.slotIndex}");
            InventorySystem.Instance.SwapSlots(fromSlot.slotIndex, toSlot.slotIndex);
        }

        draggingSlot.iconImage.raycastTarget = true;
        draggingSlot = null;
        RefreshInventory(); // redraw UI
    }

    private InventorySlotUI GetSlotUnderMouse()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var slotUI = result.gameObject.GetComponentInParent<InventorySlotUI>();
            if (slotUI != null && slotUI != draggingSlot)
                return slotUI;
        }

        return null;
    }
}