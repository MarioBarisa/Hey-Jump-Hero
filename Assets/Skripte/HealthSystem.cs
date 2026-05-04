using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // dodaj ovo

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private TextMeshProUGUI _healthText; // povuci iz Inspectora

    private int _currentHealth;
    private bool canTakeDamage = true;
    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _flashDuration = new WaitForSeconds(0.5f);

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
        _currentHealth -= damage;
        UpdateHealthText(); 
        StartCoroutine(FlashRed());
        if (_currentHealth <= 0) Die();
    }

    private void UpdateHealthText()
    {
        _healthText.text = $" ♥️{_currentHealth} / ♥️{_maxHealth}";
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
}