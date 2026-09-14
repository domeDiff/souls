using UnityEngine;
using UnityEngine.UIElements;

public class playercontroller : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int p_attackDamage = 100;
    [SerializeField] private float p_attackRange = 3f;

    private Rigidbody rb;
    private InputSystem_Actions inputActions;

    private bossHealth BossHealth;

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
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 movement = new Vector3(input.x, 0f, input.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Update()
    {
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

        if (distance <= p_attackRange)
        {
            bossHealth health = bossObject.GetComponent<bossHealth>();

            if (health != null)
            {
                health.TakeDamage(p_attackDamage);
                Debug.Log("player attacked boss");
            }
        }

        else
        {
            Debug.Log("too far");
        }
    }
}
