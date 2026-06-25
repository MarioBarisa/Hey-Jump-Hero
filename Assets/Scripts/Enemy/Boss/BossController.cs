using UnityEngine;
public class BossController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private DamageSystem damageSystem;
    private Animator animator;
    private bool movingRight = true;
    private bool isAttacking = false;
    private bool playerInRange = false;

    public void DealDamage()
    {
        damageSystem.canDamage = true;
    }

    private void DisableDamage()
    {
        damageSystem.canDamage = false;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetBool("isAttacking", true);
                Invoke(nameof(StopAttack), 1.5f);
            }
        }
        else
        {
            if (!isAttacking)
                Patrol();
        }
    }

    private void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        damageSystem.canDamage = false;
    }

    private void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= rightEdge.position.x)
            {
                movingRight = false;
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= leftEdge.position.x)
            {
                movingRight = true;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}