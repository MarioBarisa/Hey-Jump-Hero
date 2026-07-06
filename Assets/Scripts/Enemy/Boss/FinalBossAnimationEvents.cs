using UnityEngine;

public class FinalBossAnimationEvents : MonoBehaviour
{
    [SerializeField] private FinalBossController boss;

    public void EnableDamage()
    {
        boss.EnableDamage();
    }

    public void DisableDamage()
    {
        boss.DisableDamage();
    }
}
