using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float poisonDuration= 3f;
    [SerializeField] private int poisonDamagePerTick= 5;
    [SerializeField] private float tickInterval= 0.5f;

    private float direction;
    private bool hit;
    private BoxCollider2D boxCollider;
    private int damage;

    public void SetDamage(int _damage)
    {
        damage= _damage;
    }
    
    private void Awake()
    {
        boxCollider= GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(hit) return;

        transform.position+= Vector3.right * speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            hit = true;
            boxCollider.enabled = false;
            Invoke(nameof(Deactivate), 0.1f);
            return;
        }

        if (collision.isTrigger && !collision.TryGetComponent(out HealthSystem _))
            return;

        if(collision.TryGetComponent(out HealthSystem health))
        {
            health.TakeDamage(damage);
            health.ApplyPoison(poisonDuration, poisonDamagePerTick, tickInterval);
        }
        
        hit= true;
        boxCollider.enabled= false;
        Invoke(nameof(Deactivate), 0.1f);
    }

    public void SetDirection(float _direction)
    {
        direction= _direction;
        gameObject.SetActive(true);
        hit= false;
        boxCollider.enabled= true; 
        CancelInvoke();
        Invoke(nameof(Deactivate), 3f);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
