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
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x <= leftEdge.position.x)
            {
                movingRight = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
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
}
