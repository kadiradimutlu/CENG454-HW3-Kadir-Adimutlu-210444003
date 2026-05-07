using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DefenderController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float crouchSpeed = 5f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 1.1f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1.2f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    [Header("Shooting Settings")]
    public Transform bulletSpawnPoint;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 moveInput;
    
    private float horizontalLookRotation = 0f;
    private float verticalLookRotation = 0f;
    
    private float currentSpeed;
    private bool isGrounded;
    
    private CapsuleCollider capsuleCollider;
    private float originalHeight;
    private Vector3 originalCenter;

    private IWeapon currentWeapon;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; 
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate; 

        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        if (capsuleCollider != null)
        {
            originalHeight = capsuleCollider.height;
            originalCenter = capsuleCollider.center;
        }
        else
        {
            Debug.LogError("MISSING CAPSULE COLLIDER!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentSpeed = moveSpeed;

        horizontalLookRotation = transform.eulerAngles.y;

        currentWeapon = new BasicWeapon();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return; 

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        horizontalLookRotation += mouseX;

        if (playerCamera != null)
        {
            verticalLookRotation -= mouseY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f); 
            playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        
        moveInput = (transform.right * moveX + transform.forward * moveZ).normalized;

        if (animator != null)
        {
            float horizontalSpeed = new Vector3(moveX, 0, moveZ).magnitude;
            animator.SetFloat("Speed", horizontalSpeed);
            animator.SetBool("IsJumping", !isGrounded);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (capsuleCollider != null)
            {
                capsuleCollider.height = crouchHeight;
                capsuleCollider.center = new Vector3(originalCenter.x, originalCenter.y - (originalHeight - crouchHeight) / 2f, originalCenter.z);
            }
            currentSpeed = crouchSpeed;
            
            if (animator != null) animator.SetBool("IsCrouching", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (capsuleCollider != null)
            {
                capsuleCollider.height = originalHeight;
                capsuleCollider.center = originalCenter;
            }
            currentSpeed = moveSpeed;
            
            if (animator != null) animator.SetBool("IsCrouching", false);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (currentWeapon != null && bulletSpawnPoint != null)
            {
                currentWeapon.Fire(bulletSpawnPoint);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = new DoubleShotWeapon(currentWeapon);
            Debug.Log("WEAPON UPGRADED: Double Shot Active!");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = new BasicWeapon();
            Debug.Log("WEAPON RESET: Single Shot Active!");
        }
    }

    void FixedUpdate()
    {
        Quaternion yRotation = Quaternion.Euler(0f, horizontalLookRotation, 0f);
        rb.MoveRotation(yRotation);

        Vector3 targetPosition = rb.position + moveInput * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }
}