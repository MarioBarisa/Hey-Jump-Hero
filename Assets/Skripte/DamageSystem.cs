using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public float damageAmount = 10f;
    public float knockbackForce = 5f;

private void OnTriggerStay2D(Collider2D collision) {

 if (collision.TryGetComponent<HealthSystem>(out var health))

 {

 health.TakeDamage((int)damageAmount);


 Rigidbody2D rb = collision.attachedRigidbody;

 if (rb != null) DealDamage(rb, collision.transform);

 }

}


private void OnCollisionStay2D(Collision2D collision) {

 if (collision.collider.TryGetComponent<HealthSystem>(out var health))

 {

 health.TakeDamage((int)damageAmount);


 Rigidbody2D rb = collision.rigidbody;

 if (rb != null) DealDamage(rb, collision.transform);

 }

}


private void DealDamage(Rigidbody2D rb, Transform collisionTransform) {

 float horizontal = Mathf.Sign(collisionTransform.position.x -

 transform.position.x);

 Vector2 forceDirection = new Vector2(horizontal, 0.75f).normalized;

 rb.linearVelocity = Vector2.zero;

 rb.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);

}
}
