using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomizerGrid : MonoBehaviour
{
    [Header("UI Display Components")]
    public Image characterDisplayImage; 

    // Selections initialized to 0
    private int currentSkinIndex = 0;       
    private int currentHairStyleIndex = 0;   
    private int currentHairColorIndex = 0;   

    void Start()
    {
        UpdateCharacterPreview();
    }

    // --- BUTTON GRID RECIPIENTS ---
    public void SelectSkin(int skinID) { currentSkinIndex = skinID; UpdateCharacterPreview(); }
    public void SelectHairStyle(int styleID) { currentHairStyleIndex = styleID; UpdateCharacterPreview(); }
    public void SelectHairColor(int colorID) { currentHairColorIndex = colorID; UpdateCharacterPreview(); }

    // --- AUTOMATED LOOKUP FROM RESOURCES ---
    private void UpdateCharacterPreview()
    {
        // 1. Build the target filename matching your convention
        string targetName = $"Player_{currentSkinIndex}_{currentHairStyleIndex}_{currentHairColorIndex}_Knife";

        // 2. Look inside Assets/Resources/PlayerPreviews/ for the sub-sprite asset
        // Unity's Resources.Load handles the Aseprite importer container mapping automatically this way
        Sprite loadedSprite = Resources.Load<Sprite>($"PlayerPreviews/{targetName}");

        if (loadedSprite != null)
        {
            characterDisplayImage.sprite = loadedSprite;
        }
        else
        {
            Debug.LogWarning($"Could not automatically load Aseprite sprite at: Resources/PlayerPreviews/{targetName}");
        }
    }

    // --- SAVE TO DISK ---
    public void SaveCustomizationChoices()
    {
        PlayerPrefs.SetInt("SkinChoice", currentSkinIndex);
        PlayerPrefs.SetInt("HairChoice", currentHairStyleIndex);
        PlayerPrefs.SetInt("ColorChoice", currentHairColorIndex);
        PlayerPrefs.Save();
    }
}