using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArcadeController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float runSpeed = 10f; 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera secondaryCamera;
    [Header("Look Settings")]
    [SerializeField] private float rotationSpeed = 720f; // MOVIMIENTO ARCADE

    private Rigidbody rb;
    private PlayerInputActions inputAction;
    private Vector2 moveInput;
    private Vector3 currentVelocity;
    private bool isSprinting;
    private bool isLookingBack;
    private float targetSpeed;
    
    
    void Awake()
    {

        inputAction = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        targetSpeed=baseSpeed;
    }

    void OnEnable()
    {

        if (inputAction == null) inputAction = new PlayerInputActions();

        inputAction.Enable();

        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;

        inputAction.Player.LookBack.performed += OnLookBack;
        inputAction.Player.LookBack.canceled += OnLookBack;
        
        inputAction.Player.Sprint.performed += OnSprint;
        inputAction.Player.Sprint.canceled += OnSprint;
    }

    void OnDisable()
    {
        if (inputAction != null)
        {
            inputAction.Player.Move.performed -= OnMove;
            inputAction.Player.Move.canceled -= OnMove;

            inputAction.Player.LookBack.performed -= OnLookBack;
            inputAction.Player.LookBack.canceled -= OnLookBack;

            inputAction.Player.Sprint.performed -= OnSprint;
            inputAction.Player.Sprint.canceled -= OnSprint;
            
            inputAction.Disable();
        }
    }
    private void OnLookBack(InputAction.CallbackContext context)
    {
        isLookingBack = context.ReadValueAsButton();
    // Intercambiamos el estado de las cámaras
        // Si isLookingBack es true: apaga main, activa rear.
        mainCamera.enabled = !isLookingBack;
        secondaryCamera.enabled = isLookingBack;
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
        //HandleCameraRotation();
        HandleSprint();
        HandleArcadeMovement();

        if (PlayerHealth.Instance.CurrentHealth()>5)
        {
            PlayerHealth.Instance.TakeDamage(0.05f);
            Debug.Log("Vida Actual: "+ PlayerHealth.Instance.CurrentHealth());  
        }

        
    }

    void FixedUpdate()
    {
        //HandleMovement();
       
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

 private void HandleArcadeMovement()
{
    // 1. Determinar la velocidad sin usar el operador "?"
        float speed;
        if (isSprinting)
        {
            speed = runSpeed;
        }
        else
        {
            speed = baseSpeed;
        } ///float speed = isSprinting ? runSpeed : baseSpeed; version corta


        // 2. Movimiento: Siempre basado en el frente real del Rigidbody
        Vector3 movement = transform.forward * moveInput.y * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // 3. Rotación: Siempre igual, sin importar la cámara
        float turn = moveInput.x * rotationSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}