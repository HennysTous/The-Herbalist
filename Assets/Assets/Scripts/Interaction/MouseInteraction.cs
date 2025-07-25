using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MouseInteractor : MonoBehaviour
{
    [Header("Configuration")]
    public LayerMask interactableMask;
    public Texture2D defaultCursor;
    public Texture2D interactCursor;
    public Camera mainCam;

    private IInteractable currentHover;

    [SerializeField] private PlayerControllerInput player;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        player = FindFirstObjectByType<PlayerControllerInput>();

        inputActions.Player.Interact.performed += ctx => {
            if (currentHover != null)
            {
                currentHover.Interact(player); 
            }
        };
    }

    void OnEnable() => inputActions.Player.Enable();
    void OnDisable() => inputActions.Player.Disable();

    void Update()
    {

        // Protection against nulls
        if (Mouse.current == null || mainCam == null || EventSystem.current == null) return;

        // If the mouse is hovering any UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            ResetCursor();
            return;
        }

        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, interactableMask))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (interactable != currentHover)
                {
                    currentHover = interactable;
                    Cursor.SetCursor(interactCursor, Vector2.zero, CursorMode.Auto);
                }
                return;
            }
        }

        // If it doesn't detect anything
        ResetCursor();
    }

    private void ResetCursor()
    {
        if (currentHover != null)
        {
            currentHover = null;
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}