using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform groundCheck; 

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] float groundCheckDistance, movementSpeed;

    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private float knockbackDuration = 0.2f;

    private int facingDirection;

    private bool isKnockedBack;
    private float knockbackTimer;


    private Rigidbody2D enemyRb; 

    private void Start()
    {
        enemyRb= GetComponent<Rigidbody2D>();

        facingDirection= 1;
    }

    private void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;

            }
            return;
        }
        Move();
    }


    private void Move()
    {
        Vector2 origin = groundCheck.position;

        Vector2 dir = new Vector2(facingDirection, -1f).normalized;

        bool groundDetected = Physics2D.Raycast(
            groundCheck.position,
            dir,
            groundCheckDistance,
            whatIsGround
        );

        
        if(!groundDetected)
        {
            Flip();
        } 
        else{
            enemyRb.linearVelocity= new Vector2(
                movementSpeed * facingDirection,
                enemyRb.linearVelocity.y
            );
        }
    }

public void ApplyKnockback(Vector2 hitSourcePosition)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        Vector2 direction = ((Vector2)transform.position - hitSourcePosition).normalized;
        enemyRb.linearVelocity = Vector2.zero;
        enemyRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        
    }


    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
