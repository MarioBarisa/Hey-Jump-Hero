using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class UpgradeHUD:MonoBehaviour
{
    [SerializeField] private TMP_Text meleeLabel;
    [SerializeField] private TMP_Text rangedLabel;
    [SerializeField] private TMP_Text healthLabel;
    
    [SerializeField] private DialogueLVLup dialogueLvlup;

    private void Start()
    {
        //RefreshRate()
    }

    public void Refresh()
    {
        if (dialogueLvlup == null) return;
        meleeLabel.text =  $"Melee x{dialogueLvlup.meleeUpgradeCount}";
        rangedLabel.text = $"Ranged x{dialogueLvlup.rangedUpgradeCount}";
        healthLabel.text = $"Health: x{dialogueLvlup.healthUpgradeCount}";
    }
    
    
}
