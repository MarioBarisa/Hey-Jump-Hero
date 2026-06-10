using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; 
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private TMP_Text _healthText; // povuci iz Inspectora
    [SerializeField] private Image shieldBar; // povuci iz Inspectora
    [SerializeField] private bool isPlayer = false; // FALSE ZA SVE OSIM PLAYER OBJEKTA

    [SerializeField] private bool isBoss;

[SerializeField] private GameObject happyCoinsText; // Prefab za sretni novčić
    [SerializeField] private int addHappyCoinsOnDeath = 2;

    private int _currentHealth;
    private bool canTakeDamage = true;
    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _flashDuration = new WaitForSeconds(0.5f);

    private float damageMultiplier = 1f;
    private bool shieldActive = false;

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

            string zivoti = "";
            for (int i = 0; i < _currentHealth / 10; i++) // AKO JE ZDRAVLJE 100, PRIKAZUJE 10 SRCA, AKO JE 50, PRIKAZUJE 5 SRCA, ITD.
            {                                             // dijelimo sa 10 da ne prikazujemo 100 srca vec 10 srca, 20 srca
                zivoti += "♥️ ";
            }

        _healthText.text = $" {zivoti} {_currentHealth}/{_maxHealth}"; // prikazuje broj života i postotak zdravlja

            if(isPlayer == false)
        {
             string zivotiEnemy = "";
            for (int i = 0; i < _currentHealth/2; i++) // Podjela sa 2 da se manje srca prikazuje
            {                                             
                zivotiEnemy += "♥️ ";
            }
            _healthText.text = $" {zivotiEnemy}";
        }
    }

    private IEnumerator FlashRed()
{
    if (_spriteRenderer != null)
    {
        _spriteRenderer.color = Color.red;
        yield return _flashDuration;
        _spriteRenderer.color = Color.white;
        yield return _flashDuration;
    }
    else
    {
        yield return _flashDuration;
        yield return _flashDuration;
    }

    canTakeDamage = true;
}

    private void Die()
    {
        int currentScene= SceneManager.GetActiveScene().buildIndex;
        if (isPlayer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (isBoss && currentScene + 1 < SceneManager.sceneCountInBuildSettings)
        {
            
            SceneManager.LoadScene(currentScene + 1);
            
            return;
        }
        else
        {
            CoinCollector happyCoins = FindFirstObjectByType<CoinCollector>();
            if (happyCoins != null)
            {
                happyCoins.AddCoins(addHappyCoinsOnDeath);
            }
            if (happyCoinsText != null)
            {
                GameObject textInstance = Instantiate(happyCoinsText, transform.position, Quaternion.identity);
                textInstance.GetComponentInChildren<TMP_Text>().text = $"+{addHappyCoinsOnDeath}"; // Postavi tekst na broj sretnih novčića
                Destroy(textInstance, 1f); // Uništi tekst nakon 1 sekunda
            }
            Destroy(gameObject);
        }
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