using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class DefenderController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float crouchSpeed = 5f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 1.1f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1.2f;
    public float crouchCameraDrop = 0.4f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    [Header("Shooting Settings")]
    public Transform bulletSpawnPoint;

    [Header("Triple Shot Ultimate")]
    public int tripleShotUnlockScore = 50;
    public float tripleShotDuration = 15f;
    public KeyCode ultimateKey = KeyCode.Q;
    public TextMeshProUGUI ultimateText;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 moveInput;

    private float horizontalLookRotation = 0f;
    private float verticalLookRotation = 0f;

    private float currentSpeed;
    private bool isGrounded;
    private bool isCrouching;

    private CapsuleCollider capsuleCollider;
    private float originalHeight;
    private Vector3 originalCenter;
    private Vector3 originalCameraLocalPosition;

    private IWeapon baseWeapon;
    private IWeapon currentWeapon;

    private bool ultimateUnlocked = false;
    private bool ultimateActive = false;
    private bool ultimateUsed = false;
    private float ultimateRemainingTime = 0f;

    void OnEnable()
    {
        GameManager.OnScoreChanged += HandleScoreChanged;
    }

    void OnDisable()
    {
        GameManager.OnScoreChanged -= HandleScoreChanged;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        originalHeight = capsuleCollider.height;
        originalCenter = capsuleCollider.center;

        if (playerCamera != null)
        {
            originalCameraLocalPosition = playerCamera.localPosition;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentSpeed = moveSpeed;
        horizontalLookRotation = transform.eulerAngles.y;

        baseWeapon = new BasicWeapon();
        currentWeapon = baseWeapon;

        UpdateUltimateUI();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        HandleLookInput();
        HandleMovementInput();
        HandleJumpInput();
        HandleCrouchInput();
        HandleShootingInput();
        HandleUltimateInput();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        Quaternion yRotation = Quaternion.Euler(0f, horizontalLookRotation, 0f);
        rb.MoveRotation(yRotation);

        Vector3 targetPosition = rb.position + moveInput * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    private void HandleLookInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        horizontalLookRotation += mouseX;

        if (playerCamera != null)
        {
            verticalLookRotation -= mouseY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
            playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);
        }
    }

    private void HandleMovementInput()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveInput = (transform.right * moveX + transform.forward * moveZ).normalized;
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleCrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCrouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StopCrouch();
        }
    }

    private void StartCrouch()
    {
        if (isCrouching) return;

        isCrouching = true;
        currentSpeed = crouchSpeed;

        capsuleCollider.height = crouchHeight;
        capsuleCollider.center = new Vector3(
            originalCenter.x,
            crouchHeight / 2f,
            originalCenter.z
        );

        if (playerCamera != null)
        {
            playerCamera.localPosition = originalCameraLocalPosition + Vector3.down * crouchCameraDrop;
        }

        // Visual crouch animation is intentionally disabled because the imported animation causes model offset issues.
    }

    private void StopCrouch()
    {
        if (!isCrouching) return;

        isCrouching = false;
        currentSpeed = moveSpeed;

        capsuleCollider.height = originalHeight;
        capsuleCollider.center = originalCenter;

        if (playerCamera != null)
        {
            playerCamera.localPosition = originalCameraLocalPosition;
        }

        // Visual crouch animation is intentionally disabled because the imported animation causes model offset issues.
    }

    private void HandleShootingInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentWeapon != null && bulletSpawnPoint != null && playerCamera != null)
            {
                currentWeapon.Fire(bulletSpawnPoint, playerCamera);
            }
        }
    }

    private void HandleUltimateInput()
    {
        if (ultimateActive)
        {
            ultimateRemainingTime -= Time.deltaTime;

            if (ultimateRemainingTime <= 0f)
            {
                DeactivateUltimate();
            }
            else
            {
                UpdateUltimateUI();
            }
        }

        if (Input.GetKeyDown(ultimateKey))
        {
            TryActivateUltimate();
        }
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        float horizontalSpeed = new Vector3(moveInput.x, 0f, moveInput.z).magnitude;
        animator.SetFloat("Speed", horizontalSpeed);

        // Jump animation is disabled because it creates visual offset problems with this character rig.
    }

    private void HandleScoreChanged(int newScore)
    {
        if (!ultimateUsed && !ultimateActive && newScore >= tripleShotUnlockScore)
        {
            ultimateUnlocked = true;
        }

        UpdateUltimateUI();
    }

    private void TryActivateUltimate()
    {
        if (!ultimateUnlocked || ultimateActive || ultimateUsed)
        {
            return;
        }

        ultimateActive = true;
        ultimateUsed = true;
        ultimateRemainingTime = tripleShotDuration;

        currentWeapon = new TripleShotWeapon(baseWeapon);
        UpdateUltimateUI();
    }

    private void DeactivateUltimate()
    {
        ultimateActive = false;
        ultimateUnlocked = false;
        ultimateRemainingTime = 0f;

        currentWeapon = baseWeapon;
        UpdateUltimateUI();
    }

    private void UpdateUltimateUI()
    {
        if (ultimateText == null) return;

        int currentScore = GameManager.Instance != null ? GameManager.Instance.CurrentScore : 0;

        if (ultimateActive)
        {
            ultimateText.text = "ULTIMATE: TRIPLE SHOT " + Mathf.CeilToInt(ultimateRemainingTime) + "s";
        }
        else if (ultimateUsed)
        {
            ultimateText.text = "ULTIMATE: USED";
        }
        else if (ultimateUnlocked)
        {
            ultimateText.text = "ULTIMATE: READY - PRESS Q";
        }
        else
        {
            ultimateText.text = "ULTIMATE: LOCKED " + currentScore + " / " + tripleShotUnlockScore;
        }
    }
}
