using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMelee : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRadius = 1f;
    public LayerMask enemyMask;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject hitParticleEffect;

    public int attackDamage = 2;

    private float cooldownTime = 0.5f;
    private float cooldownTimer = 0f;

    private void OnDrawGizmos()
    {
        if (attackOrigin == null) return;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0)
        {
            cooldownTimer = cooldownTime;

            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }

            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);

            foreach (Collider2D enemy in enemiesInRange)
            {
                HealthSystem health = enemy.GetComponent<HealthSystem>();
                if (health != null)
                {
                    health.TakeDamage(attackDamage);
                }

                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.ApplyKnockback(transform.position);
                }

                if (hitParticleEffect != null)  // particle system koji se aktivira kad igrač pogodi neprijatelja
                {
                    GameObject effect = Instantiate(
                        hitParticleEffect,
                        enemy.transform.position + Vector3.up * 0.5f,
                        Quaternion.identity
                    );

                    ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.Play();
                    }
                }

            }
        }
    }
}