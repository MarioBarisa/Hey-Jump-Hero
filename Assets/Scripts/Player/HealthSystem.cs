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

    [SerializeField] private bool isFinalBoss;

    [SerializeField] private GameObject happyCoinsText; // Prefab za sretni novčić
    [SerializeField] private int addHappyCoinsOnDeath = 2;

    [SerializeField] private TMP_Text upgradeAvailableLable;
    [SerializeField] private int upgradeCheckCost = 4; // ovo se mora podudarati sa cijenom upgrejda!
    [SerializeField] private CoinCollector coinCollector;

    private int _currentHealth;
    private bool canTakeDamage = true;
    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _flashDuration = new WaitForSeconds(0.5f);

    private float damageMultiplier = 1f;
    private bool shieldActive = false;

    [SerializeField] private AudioSource damageAudioSource;

    //stun (boss 3)
    private bool isStunned = false;
    
    public Slider healthBarSlider;
    public TextMeshProUGUI healthBarText;
    [SerializeField] private GameObject bossHealthBarPanel;

    [SerializeField] private AudioSource stunAudioSource;

    public void ApplyStun(float duration)
    {
        if (!isStunned)
            StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;

        if (stunAudioSource != null)
        {
            stunAudioSource.Play();
        }
        yield return new WaitForSeconds(duration);

        if (stunAudioSource != null)
        {
            stunAudioSource.Stop();
        }

        isStunned = false;
    }

    public bool IsStunned() => isStunned;

    // poison (boss 2)
    private bool isPoisoned= false;

    public void ApplyPoison(float duration, int damagePerTick, float tickInterval)
    {
        if (!isPoisoned)
        {
            StartCoroutine(PoisonCoroutine(duration, damagePerTick, tickInterval));
        }
    }

    private IEnumerator PoisonCoroutine(float duration, int damagePerTick, float tickInterval)
    {
        isPoisoned= true;
        float elapsed= 0f;
        while(elapsed < duration)
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = new Color(0.6f, 0f, 0.8f);
            
            }
            yield return new WaitForSeconds(tickInterval);

            if (_spriteRenderer != null)
            {    
                _spriteRenderer.color = Color.white;
            }
            
            elapsed+= tickInterval;
            _currentHealth-=damagePerTick;
            
            PlayDamageSound();

            UpdateHealthText();
            if(_currentHealth <= 0)
            {
                Die();
                yield break;
            }
        }
        isPoisoned= false;
    }


    // OVO JE FUNKCIJA KOJA SE POZIVA KADA IGRAČ IZABERE HEALTH U DIALOGU, POVEĆAVA MAKSIMALNO ZDRAVLJE I TRENUTNO ZDRAVLJE ZA ODREĐENI IZNOS
    public void upgradePlayerHealth(int additiveHealth)
    {
        _maxHealth += additiveHealth;
        _currentHealth += additiveHealth; // Povećaj trenutne živote za isti iznos
        UpdateHealthText();
    }

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _currentHealth = _maxHealth;
        if (coinCollector == null) coinCollector = GetComponent<CoinCollector>();
        UpdateHealthText();

        if (isBoss && bossHealthBarPanel != null)
            bossHealthBarPanel.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage) return;
        canTakeDamage = false;
        _currentHealth -= Mathf.RoundToInt(damage * damageMultiplier);

        if (isFinalBoss)
        {
            FinalBossController boss= GetComponent<FinalBossController>();

            if(boss!=null)
                boss.OnDamaged();
        }

        PlayDamageSound();
        UpdateHealthText(); 
        StartCoroutine(FlashRed());
        if (_currentHealth <= 0) Die();
    }

    private void UpdateHealthText()
    {
        
        if (isBoss)
        {
            if (healthBarSlider != null)
            {
                healthBarSlider.value = _currentHealth;
                healthBarSlider.maxValue = _maxHealth;
                healthBarSlider.minValue = 0;
            }
            
            
            if (healthBarText != null)
                healthBarText.text = $"{_currentHealth}";
        }
        
            if (_healthText == null) return;

            string zivoti = "";
            for (int i = 0; i < _currentHealth / 10; i++) // AKO JE ZDRAVLJE 100, PRIKAZUJE 10 SRCA, AKO JE 50, PRIKAZUJE 5 SRCA, ITD.
            {                                             // dijelimo sa 10 da ne prikazujemo 100 srca vec 10 srca, 20 srca
                zivoti += "♥️ ";
            }
            
            _healthText.text = $" {zivoti} {_currentHealth}/{_maxHealth}"; // prikazuje broj života i postotak zdravlja
        
        
            if(isPlayer == false){ 
                string zivotiEnemy = "";
            
                float healthPrecentage = _currentHealth / (float)_maxHealth;
                _healthText.color = Color.Lerp(Color.red, Color.green, healthPrecentage);
                
                for (int i = 0; i < _currentHealth; i++) // Podjela sa 2 da se manje srca prikazuje
            {                                             
                zivotiEnemy = i.ToString();
            }
            _healthText.text = $"HP: {zivotiEnemy}";
            
        }

        if (upgradeAvailableLable != null && coinCollector != null && isPlayer)
        {
            upgradeAvailableLable.gameObject.SetActive(coinCollector.HasEnoughCoins(upgradeCheckCost));
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
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (isBoss)
        {
            if (bossHealthBarPanel != null)
                bossHealthBarPanel.SetActive(false);

            if (currentScene + 1 < SceneManager.sceneCountInBuildSettings)
            {
                FindFirstObjectByType<FadeManager>().LoadNextLevel();
            }
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

    public void refreshUpgradeLabel()
    {
        if (upgradeAvailableLable != null && coinCollector != null && isPlayer)
        {
            upgradeAvailableLable.gameObject.SetActive(coinCollector.HasEnoughCoins(upgradeCheckCost));
        }
    }

    private void PlayDamageSound()
    {
        if (damageAudioSource != null)
        {
            damageAudioSource.Play();
        }
    }

    public bool IsBoss()
    {
        return isBoss || isFinalBoss;
    }
}