using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerExtend : MonoBehaviour
{
    [SerializeField] private Animator weaponAnimator;

    [SerializeField] private float attackCooldown = 1f;

    [SerializeField] private AudioSource audioSource;

    private float cooldownTimer;

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;

            weaponAnimator.SetTrigger("extend");
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }
}
