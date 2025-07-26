using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Text commonNameText;
    [SerializeField] private Text scientificNameText;
    [SerializeField] private Text typeText;
    [SerializeField] private Text effectText;

    public void Show(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("TooltipUI: ItemData es null");
            return;
        }
        //itemImage.sprite = item.icon;
        commonNameText.text = item.itemName;
        scientificNameText.text = item.scientificName;
        typeText.text = item.itemType.ToString();

        if (item.itemType.Equals(ItemType.Consumable) && item.speedBuff != null)
        {
            effectText.text = item.speedBuff.description;
        }
        else
        {
            effectText.text = "Without Effect";
        }


        itemImage.gameObject.SetActive(true);
    }

    public void Hide()
    {
        itemImage.gameObject.SetActive(false);
    }
}