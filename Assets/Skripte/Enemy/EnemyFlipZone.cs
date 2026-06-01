using UnityEngine;

public class EnemyFlipZone : MonoBehaviour
{
    [SerializeField] private RangedEnemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.Flip();
        }
    }
}
