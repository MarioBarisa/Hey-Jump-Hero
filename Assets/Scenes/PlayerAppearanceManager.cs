using UnityEngine;

namespace HeyJumpHero
{
    public class PlayerAppearanceManager : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        // Direct links to your Player systems to check active objects
        [Header("Weapon Object Detection")]
        [SerializeField] private GameObject rifleObject;
        [SerializeField] private GameObject extendWeaponObject;

        private int skinIndex;
        private int hairStyleIndex;
        private int hairColorIndex;

        private string lastLoadedPath = "";
        private Sprite[] cachedCustomFrames;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            // Auto-detect the weapon slots from your Player script if left unassigned
            Player playerScript = GetComponent<Player>();
            if (playerScript != null)
            {
                // Accessing the private serialized fields requires them to be assigned via inspector,
                // but we can look them up or rely on manual assignment below for 100% stability.
            }

            // Read the custom choices saved from the menu preferences registry
            skinIndex = PlayerPrefs.GetInt("SkinChoice", 0);
            hairStyleIndex = PlayerPrefs.GetInt("HairChoice", 0);
            hairColorIndex = PlayerPrefs.GetInt("ColorChoice", 0);
        }

        void LateUpdate()
        {
            if (spriteRenderer == null || spriteRenderer.sprite == null || animator == null) return;

            string currentSpriteName = spriteRenderer.sprite.name;

            // 1. CHOOSE SUFFIX BY DIRECT VISIBILITY OF THE WEAPONS
            string weaponActionState = "Knife"; 

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("melee"))
            {
                weaponActionState = "Knife_Attack";
            }
            // If the AK47 or Pike game objects are physically switched on, FORCE the "Weapon" sheet
            else if ((rifleObject != null && rifleObject.activeInHierarchy) || 
                     (extendWeaponObject != null && extendWeaponObject.activeInHierarchy))
            {
                weaponActionState = "Weapon";
            }
            else
            {
                weaponActionState = "Knife";
            }

            // 2. EXTRACT THE FRAME NUMBER INDEX
            int frameIndex = GetFrameIndexFromName(currentSpriteName);

            // 3. TARGET THE CUSTOM RESOURCES FOLDER PATH
            string targetSheetPath = $"PlayerPreviews/Player_{skinIndex}_{hairStyleIndex}_{hairColorIndex}_{weaponActionState}";

            // 4. LOAD AND CACHE THE CUSTOM SPRITE SHEET
            if (targetSheetPath != lastLoadedPath)
            {
                cachedCustomFrames = Resources.LoadAll<Sprite>(targetSheetPath);
                lastLoadedPath = targetSheetPath;
            }

            // 5. OVERRIDE TRADITIONAL TEXTURES DYNAMICALLY
            if (cachedCustomFrames != null && frameIndex >= 0 && frameIndex < cachedCustomFrames.Length)
            {
                spriteRenderer.sprite = cachedCustomFrames[frameIndex];
            }
        }

        private int GetFrameIndexFromName(string name)
        {
            int lastUnderscore = name.LastIndexOf('_');
            if (lastUnderscore != -1 && int.TryParse(name.Substring(lastUnderscore + 1), out int index))
            {
                return index;
            }
            return 0;
        }
    }
}