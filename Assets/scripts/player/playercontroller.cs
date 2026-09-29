using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class playercontroller : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private float p_attackRange = 3f;

    [Header("Movement")]
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Dodge")]
    [SerializeField] private float dodgeSpeed = 12f;
    [SerializeField] private float dodgeDuration = 0.25f;
    [SerializeField] private float dodgeCooldown = 1f;

    [SerializeField] private TrailRenderer dodgeTrail;
    private float dodgeCooldownTimer;
    private bool isDodging;
    private float dodgeTimer;
    private Vector3 dodgeDirection;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    //refernces 
    private playerHealth playerHealth;
    private Rigidbody rb;
    private InputSystem_Actions inputActions;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
        playerHealth = GetComponent<playerHealth>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        dodgeTrail.emitting = false;
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if(dodgeCooldownTimer > 0f)
        {
            dodgeCooldownTimer -= Time.fixedDeltaTime;
        }

        if (isDodging)
        {
            rb.MovePosition(rb.position + dodgeSpeed * Time.fixedDeltaTime * dodgeDirection);


            dodgeTimer -= Time.fixedDeltaTime;

            if (dodgeTimer <= 0f)
            {
                isDodging = false;
                playerHealth.SetInvincible(false);
                dodgeTrail.emitting = false;
            }


            return;
        }

        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        float currentSpeed = moveSpeed;

        if (inputActions.Player.Sprint.IsPressed() && !isDodging)
        {
            currentSpeed = sprintSpeed;
        }

        Vector3 movement = new Vector3(input.x, 0f, input.y) * currentSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }

    private void Update()
    { 
        if (inputActions.Player.Dodge.WasPressedThisFrame() && !isDodging && dodgeCooldownTimer <= 0f)
        {
            StartDodge();
        }

        if (inputActions.Player.Attack.WasPressedThisFrame())
        {
            Attack();
        }

        if (inputActions.Player.Jump.WasPressedThisFrame() && isGrounded)
        {
            Jump();
        }
    }

    private void Attack()
    {
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");

        if (bossObject == null)
            return;

        float distance = Vector3.Distance(transform.position, bossObject.transform.position);

        if(distance <= p_attackRange)
        {
            Debug.Log("player attacked");

            attackHitBox.SetActive(true);
            Invoke(nameof(DisableAttackHitBox), 0.2f);
        }
    }

    private void DisableAttackHitBox()
    {
        attackHitBox.SetActive(false);
    }

    private void StartDodge()
    {
        dodgeTrail.emitting = true;

        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        dodgeDirection = new Vector3(input.x, 0f, input.y);

        if(dodgeDirection == Vector3.zero)
        {
            dodgeDirection = transform.forward;
        }

        dodgeDirection.Normalize();

        isDodging = true;
        playerHealth.SetInvincible(true);
        dodgeTimer = dodgeDuration;
        dodgeCooldownTimer = dodgeCooldown;

        Debug.Log("player used dodge!");
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;

        Debug.Log("player jumped");
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
