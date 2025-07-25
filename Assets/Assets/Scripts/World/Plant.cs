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

        print($"{data.itemName} gathered!");
        Destroy(gameObject);
        /* TODO
         * if (InventorySystem.Instance.AddItem(data)) {
            Destroy(gameObject);
        } else {
            Debug.Log("Inventario lleno");
        }
         */
    }
}
