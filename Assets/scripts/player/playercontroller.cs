using UnityEngine;

public class playercontroller : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

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
}
