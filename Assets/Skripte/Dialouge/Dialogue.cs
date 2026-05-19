using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    //POLJA

    //PROZOR
    public GameObject window;
    //INDIKATOR

    public GameObject indicator;

    //LISTA DIJALOGA
    public List<String> dialouges;

    //BRZINA ISPISA
    public float writingSpeed;

    //tekst komponenta
    public TMP_Text dialougeText;

    private int index;

    //Character index
    public int charIndex;

    //počeo dijalog
    private bool startedDialouge;

    //Čekaj na input
    private bool waitForNext;

    private void Awake()
    {
        ToggleWindow(false);
        ToggleIndicator(false);
    }


    public void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }

    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }


    //StartDialogue
    public void startDialogue()
    {
        if (startedDialouge) return;
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

        //počni pisat
        StartCoroutine(Writing());
    }
    //EndDialogue
    public void endDialogue()
    {
        startedDialouge = false;
        ToggleWindow(false);
        ToggleIndicator(true);
    }

   IEnumerator Writing()
{
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

        if (index < dialouges.Count - 1)
        {
            index++;
            GetDialouge(index);
        }
        else
        {
            endDialogue();
        }
    }
}
}


