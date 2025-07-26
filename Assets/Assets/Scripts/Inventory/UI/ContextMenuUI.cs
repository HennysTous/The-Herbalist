using UnityEngine;
using UnityEngine.UI;

public class ContextMenuUI : MonoBehaviour
{
    [SerializeField] private Button consumeButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private RectTransform panel;

    private InventorySlot currentSlot;
    private InventorySlotUI currentUI;

    public void Show(InventorySlot slot, Vector3 position, InventorySlotUI uiRef)
    {
        currentSlot = slot;
        currentUI = uiRef;

        panel.position = position;
        gameObject.SetActive(true);

        bool isConsumable = slot.item.itemType.Equals(ItemType.Consumable);
        consumeButton.gameObject.SetActive(isConsumable);

        consumeButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();

        if (isConsumable)
        {
            consumeButton.onClick.AddListener(ConsumeItem);
        }

        deleteButton.onClick.AddListener(DeleteItem);
    }

    private void ConsumeItem()
    {
        // Aplica el efecto del buff (ej: velocidad)
        Debug.Log($"You gained {currentSlot.item.itemName}");

        InventorySystem.Instance.RemoveItem(currentSlot.item);
        InventoryUIManager.Instance.HideContextMenu();
    }

    private void DeleteItem()
    {
        InventorySystem.Instance.RemoveItem(currentSlot.item);
        InventoryUIManager.Instance.HideContextMenu();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
