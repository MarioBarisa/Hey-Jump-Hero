using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMelee : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRadius = 1f;
    public LayerMask enemyMask;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject hitParticleEffect;

    [SerializeField] private Animator  animator;

    public int attackDamage = 2;

    private float cooldownTime = 0.5f;
    private float cooldownTimer = 0f;

    private void OnDrawGizmos()
    {
        if (attackOrigin == null) return;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }

    private void StopAttackAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

    //UPGRADE ATK DAMAGE
    public void upgradeAttackDamage(int additiveDamage)
    {
        attackDamage += additiveDamage;
    }

    //SETTER ZA ATK DAMAGE (ako želimo direktno postaviti vrijednost, npr. na 10)
    public void setAttackDamage(int newDamage)
    {
        attackDamage = newDamage;
    }

    void Update()
    {
        HealthSystem playerHealth = GetComponentInParent<HealthSystem>();
        if (playerHealth != null && playerHealth.IsStunned()) return;

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0)
        {
            cooldownTimer = cooldownTime;

            animator.SetBool("isAttacking", true);

            Invoke(nameof(StopAttackAnimation), 0.2f);

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