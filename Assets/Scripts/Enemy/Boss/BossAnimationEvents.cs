using UnityEngine;
public class BossAnimationEvents : MonoBehaviour
{
    [SerializeField] private DamageSystem damageSystem;

    public void DealDamage()
    {
        damageSystem.canDamage = true;
    }

    public void StopDamage()
    {
        damageSystem.canDamage = false;
    }
}