using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
                                IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI References")]
    public Image iconImage;
    public Text quantityText;
    public Image highlightBorder;

    [HideInInspector] public int slotIndex;
    private InventorySlot slotData;
    private CanvasGroup canvasGroup;

    private Vector3 iconOriginalPos;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        iconOriginalPos = iconImage.rectTransform.localPosition;
    }

    public void SetupSlot(InventorySlot data, int index)
    {
        slotData = data;
        slotIndex = index;

        if (slotData != null && slotData.item != null)
        {
            iconImage.sprite = slotData.item.icon;
            quantityText.text = slotData.quantity > 1 ? slotData.quantity.ToString() : "";
            iconImage.enabled = true;
        }
        else
        {
            iconImage.sprite = null;
            quantityText.text = "";
            iconImage.enabled = false;
            highlightBorder.enabled = false;
        }

        ResetIconPosition();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotData?.item != null)
        {
            highlightBorder.color = Color.white;
            highlightBorder.enabled = true;
            InventoryUIManager.Instance.ShowTooltip(slotData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (slotData?.item != null)
        {
            highlightBorder.enabled = false;
            InventoryUIManager.Instance.HideTooltip();
        }

       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotData?.item != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (VendorManager.Instance.isVendorActive)
            {
                CurrencySystem.Instance.AddCoins(slotData.item.sellValue * slotData.quantity);
                InventorySystem.Instance.RemoveItem(slotData.item);
            }
            else
            {
                InventoryUIManager.Instance.ShowContextMenu(slotData, transform.position, this);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotData?.item != null && eventData.button == PointerEventData.InputButton.Left)
        {
            canvasGroup.blocksRaycasts = false;
            quantityText.gameObject.SetActive(false);
            InventoryUIManager.Instance.BeginDrag(this);
        }
            
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (slotData?.item != null && eventData.button == PointerEventData.InputButton.Left)
        {
            iconImage.rectTransform.position = eventData.position;
        }
            
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (slotData?.item != null && eventData.button == PointerEventData.InputButton.Left)
        {
            canvasGroup.blocksRaycasts = true;
            quantityText.gameObject.SetActive(true);
            InventoryUIManager.Instance.EndDrag(this);
            ResetIconPosition();
        }
            
    }

    private void ResetIconPosition()
    {
        iconImage.rectTransform.localPosition = iconOriginalPos;
    }

    public InventorySlot GetSlotData() => slotData;
}