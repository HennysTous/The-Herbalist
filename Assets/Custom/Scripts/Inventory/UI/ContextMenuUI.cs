using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuUI : MonoBehaviour
{
    [SerializeField] private Button consumeButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private RectTransform panel;

    [SerializeField] private Vector3 menuOffset;

    private InventorySlot currentSlot;
    private InventorySlotUI currentUI;
    private UpgradesManager upgradesManager;

    private void Awake()
    {
        upgradesManager = UpgradesManager.Instance;
    }

    public void Show(InventorySlot slot, Vector3 position, InventorySlotUI uiRef)
    {
        currentSlot = slot;
        currentUI = uiRef;

        panel.position = position + menuOffset;
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

        upgradesManager.ApplyUpgrade(currentSlot.item);

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
