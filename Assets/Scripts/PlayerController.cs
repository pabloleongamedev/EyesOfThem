using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float movementSmoothing = 10f;
    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float TopClamp = 90.0f;
    [SerializeField] private float BottomClamp = -90.0f;
    [Header("SFX")]
    [SerializeField] private AudioClip walk;
    [SerializeField] private AudioClip run;
    [SerializeField] private AudioClip interctButton;


    private Rigidbody rb;
    private PlayerInputActions inputAction;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 currentVelocity;
    private bool isSprinting;
    private float targetSpeed;
    private Interactable currentInteractable; // Referencia al objeto 

    private Camera mainCamera;
    private float cameraPitch;

    void Awake()
    {

        inputAction = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        targetSpeed = baseSpeed;


        StartCoroutine(ShowBanner());
    }

    void OnEnable()
    {

        if (inputAction == null) inputAction = new PlayerInputActions();

        inputAction.Enable();

        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;

        inputAction.Player.Look.performed += OnLook;
        inputAction.Player.Look.canceled += OnLook;

        inputAction.Player.Interact.performed += OnInteract;
        inputAction.Player.Interact.canceled += OnInteract;

        inputAction.Player.Sprint.performed += OnSprint;
        inputAction.Player.Sprint.canceled += OnSprint;

        inputAction.Player.Inventory.performed += OnInventory;
        inputAction.Player.Menu.performed += OnMenu;

    }


    void OnDisable()
    {
        if (inputAction != null)
        {
            inputAction.Player.Move.performed -= OnMove;
            inputAction.Player.Move.canceled -= OnMove;

            inputAction.Player.Look.performed -= OnLook;
            inputAction.Player.Look.canceled -= OnLook;

            inputAction.Player.Interact.performed -= OnInteract;
            inputAction.Player.Interact.canceled -= OnInteract;

            inputAction.Player.Sprint.performed -= OnSprint;
            inputAction.Player.Sprint.canceled -= OnSprint;

            inputAction.Player.Inventory.performed -= OnInventory;

            inputAction.Player.Menu.performed += OnMenu;


            inputAction.Disable();
        }
    }

    IEnumerator ShowBanner()
    {

        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(7f);

        UIManager.Instance.CloseMenuExplicitly();
        Time.timeScale = 1;
        GameManager.Instance.isGameActive = true;
        Cursor.lockState = CursorLockMode.Locked;

    }
    void OnMenu(InputAction.CallbackContext context)
    {
        {
            if (UIManager.Instance.SetVisibilityMenu())
            {
                Cursor.lockState = CursorLockMode.Confined;
                Time.timeScale = 0;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            ;
        }
    }
    private void OnInventory(InputAction.CallbackContext context)
    {
        if (InventoryManager.Instance.SetVisibilityInventory())
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else { Cursor.lockState = CursorLockMode.Locked; }
        ;
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            if (isSprinting) { AudioManager.Instance.PlaySFX(run); }
            else { AudioManager.Instance.PlaySFX(walk); }
        }
        else { AudioManager.Instance.StopSFX(); }
    }
    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    private void OnInteract(InputAction.CallbackContext context)
    {

        if (currentInteractable != null)
        {
            if (currentInteractable.DoInteraction()) { AudioManager.Instance.PlaySFX(interctButton, 0.5f); }

        }

    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        bool tryingToSprint = context.ReadValueAsButton();

        if (tryingToSprint && PlayerStats.Instance.CanSprint())
        {
            isSprinting = true;
            targetSpeed = runSpeed;
        }
        else
        {
            isSprinting = false;
            targetSpeed = baseSpeed;
        }

    }

    void Update()
    {
        HandleCameraRotation();
        HandleSprint();

        if (PlayerHealth.Instance.GetHealthNormalized() == 0)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    void FixedUpdate()
    {
        HandleMovement();

    }

    private void HandleMovement()
    {

        Vector3 targetDirection = (transform.forward * moveInput.y) + (transform.right * moveInput.x);

        if (targetDirection.magnitude > 1f)
        {
            targetDirection.Normalize();
            PlayerStats.Instance.ConsumeStamina(isSprinting);
        }


        Vector3 desiredVelocity = targetDirection * targetSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, movementSmoothing * Time.fixedDeltaTime);

        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);

    }

    private void HandleCameraRotation()
    {
        if (lookInput.sqrMagnitude >= 0.01f)
        {
            transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);
            cameraPitch += lookInput.y * mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, BottomClamp, TopClamp);
            mainCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        }
    }
    private void HandleSprint()
    {
        if (PlayerStats.Instance == null) return;

        if (isSprinting && moveInput.sqrMagnitude > 0)
        {
            PlayerStats.Instance.ConsumeStamina(isSprinting);

            if (!PlayerStats.Instance.CanSprint())
            {
                isSprinting = false;
                targetSpeed = baseSpeed;
            }
        }
    }
    // Método para que el Interactable se asigne a sí mismo
    public void SetCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
    }
}