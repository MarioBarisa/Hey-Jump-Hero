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
    private float cooldownTimer= Mathf.Infinity;


    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if(cooldownTimer >= attackCooldown)
            {
                RangedAttack();
            }
        }        
    }


    private void RangedAttack()
    {
        cooldownTimer= 0;

        int projectileIndex= FindProjectile();


        projectiles[projectileIndex].transform.position= projectilePoint.position;
        
        EnemyProjectile projectile =
            projectiles[projectileIndex].GetComponent<EnemyProjectile>();

        projectile.SetDamage(damage);
        projectile.SetDirection(transform.localScale.x);
    }

    private int FindProjectile()
    {
        for(int i=0; i<projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }


    private bool PlayerInSight()
    {
        RaycastHit2D hit= Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(
                boxCollider.bounds.size.x * range,
                boxCollider.bounds.size.y,
                boxCollider.bounds.size.z
            ),
            0,
            Vector2.left,
            0,
            playerLayer
        );
        return hit.collider!= null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color= Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(
                boxCollider.bounds.size.x * range,
                boxCollider.bounds.size.y,
                boxCollider.bounds.size.z
            )
        );
    }

}
