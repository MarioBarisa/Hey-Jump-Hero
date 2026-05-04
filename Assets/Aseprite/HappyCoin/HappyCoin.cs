using UnityEngine;

public class HappyCoin : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<CoinCollector>().AddCoin();
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        Destroy(gameObject);
    }
}