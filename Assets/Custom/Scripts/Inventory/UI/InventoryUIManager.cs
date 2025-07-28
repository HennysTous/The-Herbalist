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
    [SerializeField] private GameObject panel;

    public List<InventorySlotUI> uiSlots = new();
    private InventorySlotUI draggingSlot;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        InventorySystem.Instance.OnInventoryChanged += RefreshInventory;
        RefreshInventory();
    }

    // Regenerates the inventory UI to match the current data in InventorySystem.
    public void RefreshInventory()
    {
        foreach (var slot in uiSlots)
            Destroy(slot.gameObject);
        uiSlots.Clear();

        var inventory = InventorySystem.Instance.slots;

        for (int i = 0; i < inventory.Count; i++)
        {
            var uiSlot = Instantiate(slotPrefab, slotParent).GetComponent<InventorySlotUI>();
            uiSlot.SetupSlot(inventory[i], i);
            uiSlots.Add(uiSlot);
        }
    }

    
    // Displays the tooltip panel with item information.
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


    // Displays the context menu for item actions (consume, drop, etc).
    public void ShowContextMenu(InventorySlot slot, Vector3 position, InventorySlotUI uiRef)
    {
        contextMenu.Show(slot, position, uiRef);
    }

    public void HideContextMenu()
    {
        contextMenu.Hide();
    }


    // Activates the inventory UI in normal or vendor mode.
    public void OpenInventory()
    {
        panel.gameObject.SetActive(true);
        RefreshInventory();
    }

    public void CloseInventory()
    {
        panel.gameObject.SetActive(false);
    }

    // Checks if inventory is opened.
    public bool isInventoryOpen()
    {
        if (panel.gameObject.active)
        {
            return true;
        }

        return false;
    }

    #region Drag & Drop

    // Starts dragging an inventory slot.
    public void BeginDrag(InventorySlotUI slot)
    {
        draggingSlot = slot;
        slot.iconImage.raycastTarget = false;
    }

    // Ends dragging and performs a swap if a target slot is valid.
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
        RefreshInventory();
    }

    // Gets the slot currently under the mouse pointer.
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

    #endregion
}