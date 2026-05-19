using UnityEngine;
using UnityEngine.InputSystem;

public class DialougeTrigger : MonoBehaviour
{
    //Detektiranje triggera sa igračem

    public Dialogue dialogueScript;

    private bool PlayerDetected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.tag == "Player")
            {
            PlayerDetected = true;
            dialogueScript.ToggleIndicator(PlayerDetected);
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerDetected = false;
            dialogueScript.ToggleIndicator(PlayerDetected);
        }
    }
    
    void Update()
    {
        if (PlayerDetected && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            dialogueScript.startDialogue();
        }
    }

}
