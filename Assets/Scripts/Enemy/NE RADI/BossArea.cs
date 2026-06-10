using UnityEngine;

public class BossArea : MonoBehaviour
{
    [SerializeField] private BossController boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            boss.SetChasing(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            boss.SetChasing(false);
    }
}