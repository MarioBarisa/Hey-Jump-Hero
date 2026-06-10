using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    //POLJA

    //PROZOR
    public GameObject window;
    //INDIKATOR

    public GameObject indicator;

    //LISTA DIJALOGA
    [Header("Dialouges")]
    public List<String> dialouges;

    //BRZINA ISPISA
    public float writingSpeed;

    //tekst komponenta
    public TMP_Text dialougeText;
    //TROŠENJE HAPPY COINS
    [Header("Choioce buttons")]
    [SerializeField] private GameObject payButton;
    [SerializeField] private GameObject noPayButton;

    private int index;

    //Character index
    public int charIndex;

    //počeo dijalog
    private bool startedDialouge;

    //Čekaj na input
    private bool waitForNext;

    //Player coin collector
    private CoinCollector _coinCollector;

    [Header("Boss Setup")]
    [SerializeField] private GameObject bossObject;   // Boss koji je već u sceni (disabled)
    [SerializeField] private int coinCost;

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
        payButton.SetActive(show);
        noPayButton.SetActive(show);
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
        if (_coinCollector == null)
        {
            //Debug.LogError("CoinCollector je null!");
            return;
        }

        //  Debug.Log($"Pokušavam platiti {coinCost} coina");

        if (_coinCollector.SpendCoins(coinCost))
        {
            //Debug.Log("Plaćeno! Spawnam bossa...");
            bossObject.SetActive(true);
            endDialogue();
        }
        else
        {
            // Debug.LogWarning("Nema dovoljno coina!");
            endDialogue();
        }
    }

    public void OnNoPayPressed()
    {
        endDialogue();
    }


}


