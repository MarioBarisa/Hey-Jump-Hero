using UnityEngine;

public class PikeDamageController : MonoBehaviour
{
    [SerializeField] private GameObject damageObject;
    [SerializeField] private DamageSystem damageSystem;

    void Start()
    {
        damageObject.SetActive(false);
        damageSystem.canDamage = false;
    }

    public void EnableDamage()
    {
        damageObject.SetActive(true);
        damageSystem.canDamage = true;
    }

    public void DisableDamage()
    {
        damageObject.SetActive(false);
        damageSystem.canDamage = false;
    }
}
