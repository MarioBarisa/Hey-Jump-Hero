using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; 
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private TextMeshProUGUI _healthText; // povuci iz Inspectora
    [SerializeField] private Image shieldBar; // povuci iz Inspectora

    private int _currentHealth;
    private bool canTakeDamage = true;
    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _flashDuration = new WaitForSeconds(0.5f);

    private float damageMultiplier = 1f;
    private bool shieldActive = false;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentHealth = _maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage) return;
        canTakeDamage = false;
        _currentHealth -= Mathf.RoundToInt(damage * damageMultiplier);
        UpdateHealthText(); 
        StartCoroutine(FlashRed());
        if (_currentHealth <= 0) Die();
    }

    private void UpdateHealthText()
    {
         if (_healthText == null) return;

        string životi = "";
        for (int i = 0; i < _currentHealth / 10; i++) // AKO JE ZDRAVLJE 100, PRIKAZUJE 10 SRCA, AKO JE 50, PRIKAZUJE 5 SRCA, ITD.
        {                                             // dijelimo sa 10 da ne prikazujemo 100 srca vec 10 srca, 20 srca
            životi += "♥️ ";
        }

        _healthText.text = $" {životi} {_currentHealth}/{_maxHealth}"; // prikazuje broj života i postotak zdravlja
    }

    private IEnumerator FlashRed()
    {
        _spriteRenderer.color = Color.red;
        yield return _flashDuration;
        _spriteRenderer.color = Color.white;
        yield return _flashDuration;
        canTakeDamage = true;
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ActivateShield(float duration, float reduction)
    {
        if (shieldActive)
            StopCoroutine(nameof(ShieldTimer));

        damageMultiplier = 1f - reduction;
        shieldActive = true;
        StartCoroutine(nameof(ShieldTimer), duration);
    }


    private IEnumerator ShieldTimer(float duration)
    {
        if (shieldBar == null) yield break;
        
        float elapsed = 0f;
        shieldBar.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            shieldBar.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }

        shieldBar.fillAmount = 1f;
        shieldBar.gameObject.SetActive(false);
        damageMultiplier = 1f;
        shieldActive = false;
    }
}