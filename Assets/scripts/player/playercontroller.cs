using UnityEngine;
using UnityEngine.UIElements;

public class playercontroller : MonoBehaviour
{
    [SerializeField] private GameObject attackHitBox;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float p_attackRange = 3f;

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
}
