using UnityEngine;

public class Plant : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractionPrompt()
    {
        return $"Gather {data.itemName}";
    }

    public void Interact(PlayerControllerInput player)
    {
        var gatherer = player.GetComponent<PlayerGatherer>();
        gatherer?.PlayGatherAnimation();

        if (InventorySystem.Instance.AddItem(data))
        {
            Debug.Log($"{data.itemName} gathered!");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventario lleno. No se pudo recolectar.");
        }
    }
}
