using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueLVLup : MonoBehaviour
{
    //PROZOR
    public GameObject window;
    //INDIKATOR

    public GameObject indicator;

    //LISTA DIJALOGA
    [Header("Dialouges")]
    public List<String> dialouges;

    [Header("Player upgrade stats")]
    [SerializeField] private int healthUpgradeAmount;
    [SerializeField] private int meleeUpgradeAmount;
    [SerializeField] private int rangedUpgradeAmount;

    //BRZINA ISPISA
    public float writingSpeed;

    //tekst komponenta
    public TMP_Text dialougeText;
    //TROŠENJE HAPPY COINS
    [Header("Choioce buttons")]
    [SerializeField] private GameObject healthUpgradeButton;
    [SerializeField] private GameObject meleeUpgradeButton;

    [SerializeField] private GameObject rangedUpgradeButton;

    private int index;

    //Character index
    public int charIndex;

    //počeo dijalog
    private bool startedDialouge;

    //Čekaj na input
    private bool waitForNext;

    //Player coin collector
    private CoinCollector _coinCollector;
    [SerializeField] private int coinCost;

void updateCoinCost()
    {
        if(coinCost==0){
            coinCost = 4;

    }
    }
    

    [SerializeField] private PlayerMelee playerMelee;
[SerializeField] private PlayerRanged playerRanged;
    [SerializeField] private HealthSystem playerHealth;

    // brojanje upgrejda-a
private int healthUpgradeCount = 0;
private int meleeUpgradeCount = 0;
private int rangedUpgradeCount = 0;
private const int maxUpgrades = 3;


    private int selectedUpgrade = -1;

public void OnSelectHealth()
    {
    updateCoinCost();
    if (_coinCollector == null || playerHealth == null) return;
    if (healthUpgradeCount >= maxUpgrades) return;

        if (_coinCollector.SpendCoins(coinCost))
        {
            playerHealth.upgradePlayerHealth(healthUpgradeAmount);
            healthUpgradeCount++;
            ToggleChoiceButtons(true);
        }
    
    Debug.Log("Health kliknut");
    Debug.Log(playerHealth);
    Debug.Log(_coinCollector);
}

public void OnSelectMelee()
    {
    updateCoinCost();
    if (_coinCollector == null || playerMelee == null) return;
    if (meleeUpgradeCount >= maxUpgrades) return;

    if (_coinCollector.SpendCoins(coinCost))
    {
        playerMelee.upgradeAttackDamage(meleeUpgradeAmount);
        meleeUpgradeCount++;
        ToggleChoiceButtons(true);
    }
}

public void OnSelectRanged()
    {
    updateCoinCost();
    if (_coinCollector == null || playerRanged == null) return;
    if (rangedUpgradeCount >= maxUpgrades) return;

    if (_coinCollector.SpendCoins(coinCost))
    {
        playerRanged.UpgradeDamage(rangedUpgradeAmount);
        rangedUpgradeCount++;
        ToggleChoiceButtons(true);
    }
}

    private void Awake()
    {
        ToggleWindow(false);
        ToggleIndicator(false);
        ToggleChoiceButtons(false);
    }


    public void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }

    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

    private void ToggleChoiceButtons(bool show)
{
    healthUpgradeButton.SetActive(show && healthUpgradeCount < maxUpgrades);
    meleeUpgradeButton.SetActive(show && meleeUpgradeCount < maxUpgrades);
    rangedUpgradeButton.SetActive(show && rangedUpgradeCount < maxUpgrades);
}


    //StartDialogue
    public void startDialogue(CoinCollector collector)
    {
        if (startedDialouge) return;

        _coinCollector = collector;
    

        //pokaži prozor dijaloga
        startedDialouge = true;
        ToggleWindow(true);
        ToggleIndicator(false);

        GetDialouge(0);
    }


    private void GetDialouge(int i)
    {
        //postavi indexe 
        index = i;
        charIndex = 0;

        //brizanje teksta
        dialougeText.text = string.Empty;
        ToggleChoiceButtons(false);

        //počni pisat
        StartCoroutine(Writing());
    }
    //EndDialogue
    public void endDialogue()
    {
        startedDialouge = false;
        ToggleWindow(false);
        ToggleChoiceButtons(false);
        ToggleIndicator(true);
    }

    IEnumerator Writing()
    {
        waitForNext = false;
        string currentDialouge = dialouges[index];

        while (charIndex < currentDialouge.Length)
        {
            dialougeText.text += currentDialouge[charIndex];
            charIndex++;
            yield return new WaitForSecondsRealtime(writingSpeed);
        }

        waitForNext = true;
    }


    private void Update()
    {
        if (!startedDialouge) return;

        if (waitForNext && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            waitForNext = false;

            bool isLastDialouge = index >= dialouges.Count - 1;

            if (!isLastDialouge) // PROMJENA: dodaj !
            {
                index++;
                GetDialouge(index);
            }
            else
            {
                ToggleChoiceButtons(true);
            }
        }
    }

    
public void OnPayPressed()
{
    if (_coinCollector == null) return;

    if (_coinCollector.SpendCoins(coinCost))
{
    if (selectedUpgrade == 0 && playerHealth != null)
    {
        playerHealth.upgradePlayerHealth(healthUpgradeAmount);
        healthUpgradeCount++;
    }
    else if (selectedUpgrade == 1 && playerMelee != null)
    {
        playerMelee.upgradeAttackDamage(meleeUpgradeAmount);
        meleeUpgradeCount++;
    }
    else if (selectedUpgrade == 2 && playerRanged != null)
    {
        playerRanged.UpgradeDamage(rangedUpgradeAmount);
        rangedUpgradeCount++;
    }
}
    endDialogue();
}

    public void OnNoPayPressed()
    {
        endDialogue();
    }


}


