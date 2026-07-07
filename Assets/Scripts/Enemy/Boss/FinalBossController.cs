using UnityEngine;

public class FinalBossController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Damage")]
    [SerializeField] private GameObject damageObject;

    [Header("Behind Detection")]
    [SerializeField] private float behindCheckDistance = 100f;
    [SerializeField] private LayerMask playerLayer;

    private bool playerBehind;

    private bool movingRight = false;
    private bool playerInRange = false;
    private float cooldownTimer = Mathf.Infinity;

    private void Start()
    {
        if (damageObject != null)
        {
            damageObject.SetActive(false);
        }    
    }

    private void Update()
    {
        CheckPlayerBehind();
        
        cooldownTimer+= Time.deltaTime;

        if (playerInRange)
        {
            animator.SetBool("isWalking", false);

            if(cooldownTimer >= attackCooldown)
            {
                cooldownTimer=0;
                if(Random.Range(0, 2) == 0)
                {
                    animator.SetTrigger("attack1");
                }
                else
                {
                    animator.SetTrigger("attack2");
                }
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        animator.SetBool("isWalking", true);
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

            if (transform.position.x >= rightEdge.position.x)
            {
                movingRight = false;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x <= leftEdge.position.x)
            {
                movingRight = true;
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void CheckPlayerBehind()
    {
        Vector2 direction= movingRight ? Vector2.left : Vector2.right;

        RaycastHit2D hit= Physics2D.Raycast(
            transform.position,
            direction,
            behindCheckDistance,
            playerLayer
        );

        playerBehind = hit.collider != null;
    }

    public void OnDamaged()
    {
        if(!playerBehind)
            return;
        
        movingRight= !movingRight;

        if(movingRight)
            transform.localScale= new Vector3(-1, 1, 1);
        else
            transform.localScale= new Vector3(1, 1, 1);
    }

    public void SetPlayerInRange(bool value)
    {
        playerInRange= value;
    }

    public void EnableDamage()
    {
        damageObject.SetActive(true);
    }

    public void DisableDamage()
    {
        damageObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector2 direction = movingRight ? Vector2.left : Vector2.right;

        Gizmos.DrawLine(
            transform.position,
            transform.position + (Vector3)(direction * behindCheckDistance)
        );
    }
}
