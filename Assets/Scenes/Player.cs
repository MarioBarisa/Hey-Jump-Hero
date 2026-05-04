using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float runSpeed = 8;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] InputActionAsset inputActions;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private int jumpCount = 0;
    private const int maxJumps = 2;

    void OnEnable() => inputActions.FindActionMap("Player").Enable();
    void OnDisable() => inputActions.FindActionMap("Player").Disable();

    void Start()
    {
        moveAction = inputActions.FindAction("Move");
        sprintAction = inputActions.FindAction("Sprint");
        jumpAction = inputActions.FindAction("Jump");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = moveAction.ReadValue<Vector2>().x;
        float speed = sprintAction.IsPressed() ? runSpeed : walkSpeed;

        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        if (moveX != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);

        if (jumpAction.triggered && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        jumpCount = 0;
    }
}
