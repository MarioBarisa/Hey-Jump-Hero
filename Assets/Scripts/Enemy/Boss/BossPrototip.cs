using UnityEngine;

public class BossPrototip : MonoBehaviour
{

    [SerializeField] private float attackCooldown; 
    [SerializeField] private int range;
    [SerializeField] private int colliderDistance;
    [SerializeField] private CapsuleCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private DamageSystem damageSystem;

    private float cooldownTimer= Mathf.Infinity;

    private void Update()
    {
        cooldownTimer+= Time.deltaTime;
        if (PlayerInSight())
        {
            if(cooldownTimer >= attackCooldown)
            {
                cooldownTimer= 0;
                damageSystem.gameObject.SetActive(true);
            }
        }
        else if (!PlayerInSight())
        {
            damageSystem.gameObject.SetActive(false);
        }
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
