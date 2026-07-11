using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class OutroController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject outroPanel; 
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [Header("Outro Story Slides")]
    [SerializeField] private Image outroImageDisplay; 
    [SerializeField] private Sprite[] outroSlides;
    private int currentSlideIndex = 0;

    private bool isOutroActive = false;

    void Start()
    {
        if (outroPanel != null) outroPanel.SetActive(false);
        
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    public void TriggerOutroSequence()
    {
        StartCoroutine(FadeToOutroSlideshow());
    }

    void Update()
    {
        if (isOutroActive)
        {
            if ((UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame) || 
                (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame))
            {
                AdvanceSlide();
            }
        }
    }

    IEnumerator FadeToOutroSlideshow()
    {
        if (fadeCanvasGroup != null) fadeCanvasGroup.blocksRaycasts = true;


        float duration = 1.0f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (outroPanel != null) outroPanel.SetActive(true);

        currentSlideIndex = 0;
        DisplaySlide();
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / duration));
            yield return null;
        }

        if (fadeCanvasGroup != null) fadeCanvasGroup.blocksRaycasts = false;
        isOutroActive = true;
    }

    void DisplaySlide()
    {
        if (outroSlides != null && outroSlides.Length > 0 && currentSlideIndex < outroSlides.Length)
        {
            outroImageDisplay.sprite = outroSlides[currentSlideIndex];
        }
    }

    void AdvanceSlide()
    {
        currentSlideIndex++;

        if (currentSlideIndex < outroSlides.Length)
        {
            DisplaySlide();
        }
        else
        {
            isOutroActive = false;
            StartCoroutine(FadeToMainMenu());
        }
    }

    IEnumerator FadeToMainMenu()
    {
        if (fadeCanvasGroup != null) fadeCanvasGroup.blocksRaycasts = true;

        float duration = 1.0f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }
}