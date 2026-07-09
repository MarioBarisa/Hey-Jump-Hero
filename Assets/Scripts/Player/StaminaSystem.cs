using UnityEngine;
using UnityEngine.UI;

namespace HeyJumpHero
{
    public class StaminaSystem : MonoBehaviour
    {
        
        [Header("Stamina values")]
        [SerializeField] public float maxStamina=100f;

        [SerializeField] private float drainRate = 25f;
        [SerializeField] private float regenRate = 20f;
        [SerializeField] private float regenDelay=1.2f;

        [Header("UI")] 
        [SerializeField] private Slider staminaSlider;
        [SerializeField] private Image staminaFill;
        [SerializeField] private CanvasGroup staminaCanvasGroup;
        
        private Color colorNormal = new Color(0.2f, 0.85f, 0.3f);
        private Color colorLow = new Color(1f, 0.7f, 0.3f);
        private Color colorEmpty = new Color(0.9f, 0.15f, 0.15f);
        
        private float currentStamina;
        private float regenTimer;
        private bool isDepleted;


        void Start()
        {
            currentStamina = maxStamina;
            UpdateUI();
        }


        void Update()
        {
            regenTimer -= Time.deltaTime;

            if (regenTimer <= 0 && currentStamina < maxStamina)
            {
                currentStamina = Mathf.Min(maxStamina, currentStamina+regenRate*Time.deltaTime);
            }

            if (isDepleted && currentStamina >= maxStamina * 0.15f)
            {
                isDepleted = false;
            }

            UpdateUI();
        }

        public bool TryDrain(float dt)
        {
            if (isDepleted || currentStamina <= 0f)
            {
                isDepleted = true;
                regenTimer = regenDelay;
                UpdateUI();
                return false;
            }
            currentStamina = Mathf.Max(0f, currentStamina - drainRate * dt);
            regenTimer = regenDelay;

            if (currentStamina <= 0f)
            {
                isDepleted = true;
            }
            
            UpdateUI();
            return true;
        }

        public void TriggerRegen()
        {
            regenTimer = 0;
        }

        public bool IsDepleted() => isDepleted;

        void UpdateUI()
        {
            if (staminaSlider == null)
            {
                return;
            }
            
            staminaSlider.value = currentStamina/maxStamina;

            if (staminaFill == null)
            {
                return;
            }

            if (isDepleted)
            {
                staminaFill.color = colorEmpty;
            } else if (currentStamina < maxStamina * 0.15f)
            {
                staminaFill.color = colorLow;
            }
            else
            {
                staminaFill.color = colorNormal;
            }


            float fillAlpha = (currentStamina < maxStamina) ? 1f : 0.4f;
            Color c = staminaFill.color;
            c.a = fillAlpha;
            staminaFill.color = c;
        }

    }
}