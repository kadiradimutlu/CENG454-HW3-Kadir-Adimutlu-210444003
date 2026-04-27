using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DefenderController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    
    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    private Rigidbody rb;
    private Vector3 moveInput;
    private float verticalLookRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; 
        rb.useGravity = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        if (playerCamera != null)
        {
            verticalLookRotation -= mouseY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f); 
            playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveInput = (transform.right * moveX + transform.forward * moveZ).normalized;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    void Shoot()
    {
        Debug.Log("Player shot the weapon towards the crosshair!");
    }
}