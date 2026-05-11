using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform groundCheck; 

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] float groundCheckDistance, movementSpeed;

    private bool groundDetected;

    private int facingDirection;

    private Vector2 movement;

    private GameObject enemy;

    private Rigidbody2D enemyRb;

    private void Start()
    {
        enemy= gameObject;

        enemyRb= GetComponent<Rigidbody2D>();

        facingDirection= 1;
    }

    private void Update()
    {
        UpdateMovingState();
    }


    private void UpdateMovingState()
    {
        Vector2 direction = Vector2.down + Vector2.right * facingDirection;
        direction.Normalize();

        groundDetected = Physics2D.Raycast(
            groundCheck.position,
            direction,
            groundCheckDistance,
            whatIsGround
        );

        if(!groundDetected){
            Flip();
        } else{
            movement.Set(
                movementSpeed * facingDirection,
                enemyRb.linearVelocity.y
            );
            enemyRb.linearVelocity= movement;
        }
    } 


    private void Flip()
    {
        facingDirection *= -1;
        enemy.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
