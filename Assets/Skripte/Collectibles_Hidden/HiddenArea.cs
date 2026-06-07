using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenArea : MonoBehaviour
{
   public Tilemap hiddenTilemap;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ENTER");
            Color c = hiddenTilemap.color;
            c.a = 0.3f; // a kod boje je alpha odnosno transparentnost
            hiddenTilemap.color = c;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("EXIT");
            Color c = hiddenTilemap.color;
            c.a = 1f;
            hiddenTilemap.color = c;
        }
    }
}
