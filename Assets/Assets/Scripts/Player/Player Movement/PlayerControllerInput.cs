using UnityEngine;
using UnityEngine.EventSystems;

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
    private PlayerAnimationHandler animHandler;

    private Vector2 moveInput;
    private Vector3 velocity;
    private float smoothVelocity;
    private bool isSprinting;
    private bool isJumpPressed;
    private bool isGrounded;

    public float CurrentSpeed { get; private set; }
    public Vector3 MoveDirection { get; private set; }

    #region Initialization

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animHandler = GetComponent<PlayerAnimationHandler>();
        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;

        inputActions.Player.Sprint.performed += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;

        inputActions.Player.Jump.performed += _ => isJumpPressed = true;

        inputActions.Player.OpenInventory.performed += _ => ToggleInventory();

        inputActions.Player.Pause.performed += _ => PauseManager.Instance.TogglePause();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    #endregion

    #region Update Loop

    private void Update()
    {
        isGrounded = controller.isGrounded;

        HandleGravityAndJump();

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            HandleMovement();
        }
        else
        {
            MoveDirection = Vector3.zero;
            animHandler.SetSpeed(0f);
        }

        Vector3 horizontal = MoveDirection.normalized * CurrentSpeed;
        controller.Move((horizontal + velocity) * Time.deltaTime);

        animHandler.SetSpeed(MoveDirection.magnitude);
        animHandler.SetSprinting(isSprinting);
    }

    #endregion

    #region Movement & Gravity

    private void HandleMovement()
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

        // Calculate current speed including permanent upgrades
        float speedMultiplier = UpgradesManager.Instance.TotalSpeedMultiplier;
        CurrentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f) * speedMultiplier;
    }

    private void HandleGravityAndJump()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // Keeps the character grounded

        if (isGrounded && isJumpPressed)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            isJumpPressed = false;
            animHandler.TriggerJump();
        }

        velocity.y += gravity * Time.deltaTime;
    }

    #endregion

    #region Inventory Toggle

    private void ToggleInventory()
    {

        if (InventoryUIManager.Instance.isInventoryOpen())

            InventoryUIManager.Instance.CloseInventory();
        else

            InventoryUIManager.Instance.OpenInventory();
    }

    #endregion
}
