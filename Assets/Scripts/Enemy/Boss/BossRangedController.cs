using UnityEngine;
public class BossRangedController : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Ranged Attack")]
    [SerializeField] private Transform projectilePoint;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private int projectileDamage = 10;
    [SerializeField] private float attackCooldown = 4f;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private LayerMask playerLayer;

    private bool movingRight = true;
    private bool isShooting = false;
    private float cooldownTimer = Mathf.Infinity;

    

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (!isShooting && cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0f;
                isShooting = true;
                Shoot();
                Invoke(nameof(StopShooting), 1f);
            }
        }
        else
        {
            if (!isShooting)
                Patrol();
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                projectiles[i].transform.position = projectilePoint.position;
                BossProjectile p = projectiles[i].GetComponent<BossProjectile>();
                p.SetDamage(projectileDamage);
                p.SetDirection(transform.localScale.x > 0 ? 1f : -1f);
                return;
            }
        }
    }

    private void StopShooting()
    {
        isShooting = false;
    }

    private void Patrol()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= rightEdge.position.x)
            {
                movingRight = false;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= leftEdge.position.x)
            {
                movingRight = true;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            movingRight ? Vector2.right : Vector2.left,
            sightRange,
            playerLayer
        );
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 dir = movingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(dir * sightRange));
    }
}