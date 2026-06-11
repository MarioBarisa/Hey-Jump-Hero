using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    public void LoadNextLevel()
    {
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        Color color = fadeImage.color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime;
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }
}