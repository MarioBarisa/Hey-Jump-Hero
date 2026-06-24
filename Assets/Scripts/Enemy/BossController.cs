using UnityEngine;
public class BossController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    private Animator animator;
    private bool movingRight = true;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private bool playerInRange = false;

    [SerializeField] private DamageSystem damageSystem;

    public void DealDamage()
    {
        damageSystem.gameObject.SetActive(true);
        Invoke(nameof(DisableDamage), 0.2f);
    }

    private void DisableDamage()
    {
        damageSystem.gameObject.SetActive(false);
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            attackTimer += Time.deltaTime;
            if (!isAttacking)
            {
                attackTimer = 0f;
                isAttacking = true;
                Debug.Log("SetBool isAttacking TRUE");
                animator.SetBool("isAttacking", true);
                Invoke(nameof(StopAttack), 1.5f);
            }
        }
        else
        {
            attackTimer = 0f;
            if (!isAttacking)
                Patrol();
        }
    }

    private void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    private void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            transform.localScale = new Vector3(1, 1, 1);
            if (transform.position.x >= rightEdge.position.x) movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            transform.localScale = new Vector3(-1, 1, 1);
            if (transform.position.x <= leftEdge.position.x) movingRight = true;
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