using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform groundCheck; 

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] float groundCheckDistance, movementSpeed;

    private int facingDirection;


    private Rigidbody2D enemyRb; 

    private void Start()
    {
        enemyRb= GetComponent<Rigidbody2D>();

        facingDirection= 1;
    }

    private void Update()
    {
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


    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
