using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject customisationPanel;
    public GameObject introPanel;
    public CanvasGroup fadeCanvasGroup; // For the fade out

    [Header("Customization Reference")]
    // Drag your Character Customization Panel or wherever the script lives here!
    public CharacterCustomizerGrid customizerGrid;

    [Header("Intro Story Slides")]
    public Image introImageDisplay; 
    public Sprite[] introSlides;   
    private int currentSlideIndex = 0;

    [Header("Scene Config")]
    public string gameplaySceneName = "Level1";

    void Start()
    {
        // Make sure panels start in the correct state
        mainMenuPanel.SetActive(true);
        customisationPanel.SetActive(false);
        introPanel.SetActive(false);
        
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    // --- BUTTON METHODS ---

    // Hook this up to your Play button
    public void OnPlayButtonPressed()
    {
        mainMenuPanel.SetActive(false);
        customisationPanel.SetActive(true);
    }

    // Hook this up to a "Back" button inside your Customisation panel
    public void OnBackButtonFromCustomizer()
    {
        customisationPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // Hook this up to the "Continue" button inside your Customisation panel
    public void OnContinueToIntro()
    {
        // 1. TELL THE CUSTOMIZER GRID TO SAVE TO PLAYERPREFS BEFORE LEAVING!
        if (customizerGrid != null)
        {
            customizerGrid.SaveCustomizationChoices();
            Debug.Log("[MainMenu] Character customization saved successfully!");
        }
        else
        {
            Debug.LogWarning("[MainMenu] Customizer Grid reference is missing! Selections won't save.");
        }

        // 2. Clear panel and advance to story slideshow
        customisationPanel.SetActive(false);
        introPanel.SetActive(true);
        
        currentSlideIndex = 0;
        DisplaySlide();
    }

    // Hook this up to your Quit button
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // --- INTRO SLIDESHOW LOGIC ---

   void Update()
    {
        if (introPanel.activeInHierarchy)
        {
            if ((UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame) || 
                (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame))
            {
                AdvanceSlide();
            }
        }
    }

    void DisplaySlide()
    {
        if (introSlides.Length > 0 && currentSlideIndex < introSlides.Length)
        {
            introImageDisplay.sprite = introSlides[currentSlideIndex];
        }
    }

    void AdvanceSlide()
    {
        currentSlideIndex++;

        if (currentSlideIndex < introSlides.Length)
        {
            DisplaySlide();
        }
        else
        {
            StartCoroutine(FadeAndLoadGame());
        }
    }

    IEnumerator FadeAndLoadGame()
    {
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.blocksRaycasts = true;

        float duration = 1.0f; 
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            }
            yield return null;
        }

        SceneManager.LoadScene(gameplaySceneName);
    }
}