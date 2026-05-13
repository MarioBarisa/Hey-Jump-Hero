using UnityEngine;

public class ShieldPowerup : MonoBehaviour
{

    [SerializeField] float duration = 20f;
    [SerializeField] float damageReduction = 0.3f;

    [SerializeField] private AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.ActivateShield(duration, damageReduction);
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
