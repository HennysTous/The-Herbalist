using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string npcName = "Charlie Green Jr";

    public string GetInteractionPrompt()
    {
        return $"Talk to {npcName}";
    }

    public void Interact(PlayerControllerInput player)
    {
        VendorManager.Instance.OpenVendor();
    }
}
