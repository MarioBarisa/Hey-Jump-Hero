using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerRanged : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform projectilePoint;
    [SerializeField] private GameObject[] projectiles;
    private float cooldownTimer = Mathf.Infinity;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer >= attackCooldown)
        {
            RangedAttack();
        }
    }

    private void RangedAttack()
    {
        cooldownTimer = 0;
        int projectileIndex = FindProjectile();
        projectiles[projectileIndex].transform.position = projectilePoint.position;
        PlayerProjectile projectile = projectiles[projectileIndex].GetComponent<PlayerProjectile>();
        projectile.SetDamage(damage);
        projectile.SetDirection(transform.localScale.x);
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
}