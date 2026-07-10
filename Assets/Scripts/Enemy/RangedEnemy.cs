using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform projectilePoint;
    [SerializeField] private GameObject[] projectiles;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private SpriteRenderer spriteRenderer;   // ← samo ovo dodajemo

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                RangedAttack();
            }
        }
    }

    private void RangedAttack()
    {
        cooldownTimer = 0;
        int projectileIndex = FindProjectile();

        projectiles[projectileIndex].transform.position = projectilePoint.position;

        EnemyProjectile projectile = projectiles[projectileIndex].GetComponent<EnemyProjectile>();
        projectile.SetDamage(damage);

        // Koristimo trenutni flip stanje spritea
        float dir = spriteRenderer.flipX ? -1f : 1f;
        projectile.SetDirection(dir);
    }

    private int FindProjectile()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool PlayerInSight()
    {
        float dir = spriteRenderer.flipX ? -1f : 1f;

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * dir * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,
            Vector2.left,   // ostavljamo originalno ili možeš koristiti new Vector2(dir, 0)
            0,
            playerLayer
        );

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        float dir = (spriteRenderer != null && spriteRenderer.flipX) ? -1f : 1f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * dir * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }

    // MINIMALNI FLIP (kao u basic Enemy) 
    public void Flip()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.flipX = !spriteRenderer.flipX;

        // Flip projectilePoint (kao groundCheck)
        if (projectilePoint != null)
        {
            projectilePoint.localPosition = new Vector2(
                -projectilePoint.localPosition.x, 
                projectilePoint.localPosition.y
            );
        }
    }
}