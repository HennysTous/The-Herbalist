using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator animator;

    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int SprintHash = Animator.StringToHash("IsSprinting");
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private readonly int GatherHash = Animator.StringToHash("IsGathering");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSpeed (float speed)
    {
        animator.SetFloat(SpeedHash, speed);
    }

    public void SetSprinting (bool isSprinting)
    {
        animator.SetBool(SprintHash, isSprinting);
    }

    public void TriggerJump()
    {
        animator.SetTrigger(JumpHash);
    }

    public void SetGathering(bool isGathering)
    {
        animator.SetTrigger(GatherHash);
    }
}
