using UnityEngine;

public class PikeDamageController : MonoBehaviour
{
    [SerializeField]
    private DamageSystem damage;

    void Start()
    {
        damage.canDamage = false;
    }

    public void EnableDamage()
    {
        damage.canDamage = true;
    }

    public void DisableDamage()
    {
        damage.canDamage = false;
    }
}
