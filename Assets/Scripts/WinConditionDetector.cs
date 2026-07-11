using UnityEngine;

public class WinConditionDetector : MonoBehaviour
{
    [Header("Target to Watch")]
    [SerializeField] private GameObject bossGameObject;

    private bool triggered = false;

    void Update()
    {
        if (triggered) return;

        if (bossGameObject == null || !bossGameObject.activeInHierarchy)
        {
            triggered = true;
            TriggerOutro();
        }
    }

    private void TriggerOutro()
    {
        Debug.Log("Boss tracker noticed the boss is gone! Launching ending sequence...");
        
        OutroController outro = Object.FindFirstObjectByType<OutroController>();
        if (outro != null)
        {
            outro.TriggerOutroSequence();
        }
    }
}