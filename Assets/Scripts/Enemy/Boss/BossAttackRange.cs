using UnityEngine;

public class BossAttackRange : MonoBehaviour
{
    [SerializeField] private FinalBossController boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetPlayerInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetPlayerInRange(false);
        }
    }
}
