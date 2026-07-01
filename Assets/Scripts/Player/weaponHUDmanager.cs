using UnityEngine;
using UnityEngine.UI;

public class weaponHUDmanager : MonoBehaviour
{
    [SerializeField] private Image[] slotBorders;
    [SerializeField] private Color activeColor =  new Color(1f, 0.85f, 0f, 1f);
    [SerializeField] private Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);
    
    private int currentSlot = 0;

    void start() => SetActiveSlot(0);

    public void SetActiveSlot(int slot)
    {
        currentSlot = slot;
        for (int i = 0; i < slotBorders.Length; i++)
        {
            slotBorders[i].color = i == currentSlot ? activeColor : inactiveColor;
        }
    }

}
