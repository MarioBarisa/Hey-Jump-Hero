using UnityEngine;

namespace HeyJumpHero
{
    public class PlayerAppearanceManager : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Animator animator;

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

            skinIndex = PlayerPrefs.GetInt("SkinChoice", 0);
            hairStyleIndex = PlayerPrefs.GetInt("HairChoice", 0);
            hairColorIndex = PlayerPrefs.GetInt("ColorChoice", 0);
        }

        void LateUpdate()
        {
            if (spriteRenderer == null || spriteRenderer.sprite == null || animator == null) return;

            // 1. DETERMINE THE EXACT FILE SUFFIX REQUIRED
            string weaponActionState = "Knife"; 

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // If the animator parameter is attacking, or we're playing/transitioning into melee
            if (animator.GetBool("isAttacking") || stateInfo.IsName("melee"))
            {
                weaponActionState = "Knife_Attack";
            }
            else if ((rifleObject != null && rifleObject.activeInHierarchy) || 
                     (extendWeaponObject != null && extendWeaponObject.activeInHierarchy))
            {
                weaponActionState = "Weapon";
            }
            else
            {
                weaponActionState = "Knife";
            }

            // 2. TARGET THE PATH
            string targetSheetPath = $"PlayerPreviews/Player_{skinIndex}_{hairStyleIndex}_{hairColorIndex}_{weaponActionState}";

            // 3. LOAD AND CACHE THE CUSTOM SHEET
            if (targetSheetPath != lastLoadedPath)
            {
                Sprite[] newFrames = Resources.LoadAll<Sprite>(targetSheetPath);
                if (newFrames != null && newFrames.Length > 0)
                {
                    // Sort frames numerically by name to ensure frame 0 matches element 0
                    System.Array.Sort(newFrames, (a, b) => AlphanumericStringComparer(a.name, b.name));
                    cachedCustomFrames = newFrames;
                    lastLoadedPath = targetSheetPath;
                }
                else
                {
                    Debug.LogWarning($"[Appearance] Could not load sheet at Assets/Resources/{targetSheetPath}");
                }
            }

            // 4. FIND THE CORRECT TIMELINE FRAME INDEX SAFELY
            if (cachedCustomFrames != null && cachedCustomFrames.Length > 0)
            {
                // Get the frame index from the default sprite currently being fed by the animation clip
                int frameIndex = GetFrameIndexFromSprite(spriteRenderer.sprite);

                // Clamp index inside our bounds to prevent out-of-bounds crashes
                frameIndex = Mathf.Clamp(frameIndex, 0, cachedCustomFrames.Length - 1);

                // Perform the absolute swap
                spriteRenderer.sprite = cachedCustomFrames[frameIndex];
            }
        }

        private int GetFrameIndexFromSprite(Sprite currentSprite)
        {
            string spriteName = currentSprite.name;
            
            // Extract numbers from the very end of the sprite name string safely
            int lastUnderscore = spriteName.LastIndexOf('_');
            if (lastUnderscore != -1 && int.TryParse(spriteName.Substring(lastUnderscore + 1), out int index))
            {
                return index;
            }
            return 0;
        }

        // Custom sorting helper to ensure frame_10 doesn't sort before frame_2 alphabetically
        private int AlphanumericStringComparer(string x, string y)
        {
            return x.CompareTo(y);
        }
    }
}