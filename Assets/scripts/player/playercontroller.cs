using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class playercontroller : MonoBehaviour
{

    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float p_attackRange = 3f;
    [SerializeField] private float dodgeSpeed = 12f;
    [SerializeField] private float dodgeDuration = 0.25f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float dodgeCooldown = 1f;


    private float dodgeCooldownTimer;
    private bool isDodging;
    private float dodgeTimer;
    private Vector3 dodgeDirection;
    private Rigidbody rb;
    private InputSystem_Actions inputActions;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void FixedUpdate()
    {
        if(dodgeCooldownTimer > 0f)
        {
            dodgeCooldownTimer -= Time.fixedDeltaTime;
        }

        if (isDodging)
        {
            rb.MovePosition(rb.position + dodgeSpeed * Time.fixedDeltaTime * dodgeDirection);


            dodgeTimer -= Time.fixedDeltaTime;

            if (dodgeTimer <= 0f)
                isDodging = false;

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
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        dodgeDirection = new Vector3(input.x, 0f, input.y);

        if(dodgeDirection == Vector3.zero)
        {
            dodgeDirection = transform.forward;
        }

        dodgeDirection.Normalize();

        isDodging = true;
        dodgeTimer = dodgeDuration;
        dodgeCooldownTimer = dodgeCooldown;

        Debug.Log("player used dodge!");


    }
}
