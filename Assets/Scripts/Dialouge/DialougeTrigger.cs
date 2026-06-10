using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // dohvat CoinCollector s Playera i proslijedi Dialogu
        CoinCollector collector = other.GetComponent<CoinCollector>();
        dialogue.startDialogue(collector);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        dialogue.endDialogue();
    }
}