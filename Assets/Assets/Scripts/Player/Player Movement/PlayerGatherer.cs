using UnityEngine;

public class PlayerGatherer : MonoBehaviour
{
    private PlayerAnimationHandler animationHandler;

    private void Awake()
    {
        animationHandler = GetComponent<PlayerAnimationHandler>();
    }

    public void PlayGatherAnimation()
    {
        animationHandler.SetGathering(true);
        ResetGathering();
    }

    private void ResetGathering()
    {
        animationHandler.SetGathering(false);
    }
}
