using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "Charlie Green Jr";

    public string Name => npcName;

    public string GetInteractionPrompt()
    {
        return $"Talk to {npcName}";
    }

    public void Interact(PlayerControllerInput player)
    {
        print($"You've talked to {npcName}");
    }
}
