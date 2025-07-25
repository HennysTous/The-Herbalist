using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerInput : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float sprintMultiplier = 1.5f;
    public float rotationSmoothTime = 0.1f;

    [Header("Gravity and Jumpforce")]
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("References")]
    public Transform cameraTransform;


    private CharacterController controller;
    private InputSystem_Actions inputActions;

    private Vector2 moveInput;
    private Vector3 velocity;
    private float smoothVelocity;
    private bool isSprinting;
    private bool isJumpPressed;
    private bool isGrounded;

    private PlayerAnimationHandler animHandler;

    public float CurrentSpeed => isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
    public Vector3 MoveDirection { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();

        animHandler = GetComponent<PlayerAnimationHandler>();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;

        inputActions.Player.Sprint.performed += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;

        inputActions.Player.Jump.performed += _ => isJumpPressed = true;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        //Movement and Jumping
        isGrounded = controller.isGrounded;

        HandleMovement();
        HandleGravityAndJump();

        //Animation Handler
        animHandler.SetSpeed(MoveDirection.magnitude);
        animHandler.SetSprinting(isSprinting);
    }

    void HandleMovement()
    {
        Vector3 camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = cameraTransform.right;

        MoveDirection = camForward * moveInput.y + camRight * moveInput.x;

        if (MoveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        Vector3 horizontal = MoveDirection.normalized * CurrentSpeed;
        controller.Move((horizontal + velocity) * Time.deltaTime);
    }

    void HandleGravityAndJump()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small force to maintain contact with the surface
        }

        if (isGrounded && isJumpPressed)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            isJumpPressed = false;
            animHandler.TriggerJump();
        }

        velocity.y += gravity * Time.deltaTime;
    }
}
