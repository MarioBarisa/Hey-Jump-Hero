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
    public CanvasGroup fadeCanvasGroup;

    [Header("Customization Reference")]
    public CharacterCustomizerGrid customizerGrid;

    [Header("Intro Story Slides")]
    public Image introImageDisplay; 
    public Sprite[] introSlides;   
    private int currentSlideIndex = 0;

    [Header("Audio Settings")]
    public AudioSource menuAudioSource;

    [Header("Scene Config")]
    public string gameplaySceneName = "Level1";

    void Start()
    {
        mainMenuPanel.SetActive(true);
        customisationPanel.SetActive(false);
        introPanel.SetActive(false);
        
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
        if (menuAudioSource != null)
        {
            menuAudioSource.loop = true;
            if (!menuAudioSource.isPlaying)
            {
                menuAudioSource.Play();
            }
        }
    }

    public void OnPlayButtonPressed()
    {
        mainMenuPanel.SetActive(false);
        customisationPanel.SetActive(true);
    }

    public void OnBackButtonFromCustomizer()
    {
        customisationPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnContinueToIntro()
    {

        if (customizerGrid != null)
        {
            customizerGrid.SaveCustomizationChoices();
            Debug.Log("[MainMenu] Character customization saved successfully!");
        }
        else
        {
            Debug.LogWarning("[MainMenu] Customizer Grid reference is missing! Selections won't save.");
        }

        customisationPanel.SetActive(false);
        introPanel.SetActive(true);
        
        currentSlideIndex = 0;
        DisplaySlide();
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


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

        float startVolume = menuAudioSource != null ? menuAudioSource.volume : 1.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / duration);

            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = normalizedTime;
            }
        
            if (menuAudioSource != null)
            {
                menuAudioSource.volume = Mathf.Lerp(startVolume, 0f, normalizedTime);
            }

            yield return null;
        }
        if (menuAudioSource != null)
        {
            menuAudioSource.Stop();
        }

        SceneManager.LoadScene(gameplaySceneName);
    }
}