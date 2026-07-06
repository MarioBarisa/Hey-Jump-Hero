using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameObject shieldObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            shieldObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            shieldObject.SetActive(true);
        }
    }
}
