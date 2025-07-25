using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform cameraPivot;
    public Transform cameraTransform;
    public float sensitivity = 1.5f;
    public float verticalMin = -40f;
    public float verticalMax = 70f;

    [Header("Shoulder Offset")]
    public Vector3 cameraOffset = new Vector3(0.8f, 1.6f, -3.5f); // x = lateral, y = height, z = distance

    private float yaw;
    private float pitch;
    private Vector2 lookInput;
    private bool isRotating = false;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += _ => lookInput = Vector2.zero;

        inputActions.Player.RotateCamera.performed += _ => isRotating = true;
        inputActions.Player.RotateCamera.canceled += _ => isRotating = false;
    }

    void OnEnable() => inputActions.Player.Enable();
    void OnDisable() => inputActions.Player.Disable();

    void LateUpdate()
    {
        if (isRotating)
        {
            yaw += lookInput.x * sensitivity;
            pitch -= lookInput.y * sensitivity;
            pitch = Mathf.Clamp(pitch, verticalMin, verticalMax);
        }

        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 offset = cameraPivot.right * cameraOffset.x + Vector3.up * cameraOffset.y - cameraPivot.forward * Mathf.Abs(cameraOffset.z);
        Vector3 targetPos = cameraPivot.position + offset;

        cameraTransform.position = targetPos;
        cameraTransform.LookAt(cameraPivot);
    }
}

