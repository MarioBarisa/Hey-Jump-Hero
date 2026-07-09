using UnityEngine;
using UnityEngine.InputSystem;

namespace HeyJumpHero
{
public class Player : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float runSpeed = 8;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] InputActionAsset inputActions;

    [SerializeField] private PlayerMelee meleeScript;
    [SerializeField] private PlayerRanged rangedScript;

    [SerializeField] private GameObject rifleObject;



    [SerializeField] private GameObject extendWeaponObject;
    [SerializeField] private Animator extendWeaponAnimator;
    [SerializeField] private PlayerExtend playerExtend;

    
    [SerializeField] private weaponHUDmanager weaponHUD;
    
    //stamina system 
    [SerializeField] private StaminaSystem staminaSystem;
    
    
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private int jumpCount = 0;
    private const int maxJumps = 1;
    
    //infinite fall prevention
    private float fallTimer = 0f;
    [SerializeField] private float maxFallTimer = 2.5f;
    private bool isDead = false;

    void OnEnable() => inputActions.FindActionMap("Player").Enable();
    void OnDisable() => inputActions.FindActionMap("Player").Disable();

    void Start()
    {
        isDead = false; //da fall timer radi nakon respawna
        moveAction = inputActions.FindAction("Move");
        sprintAction = inputActions.FindAction("Sprint");
        jumpAction = inputActions.FindAction("Jump");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        weaponHUD?.SetActiveSlot(0);
    }

    void Update()
    {
        HealthSystem health = GetComponent<HealthSystem>();
        if (health != null && health.IsStunned())
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        float moveX = moveAction.ReadValue<Vector2>().x;
        float speed;
        
        //STAMINA SYS
        bool wantsSprint = sprintAction.IsPressed() && Mathf.Abs(moveAction.ReadValue<Vector2>().x) > 0.1f;

        if (wantsSprint && staminaSystem != null && staminaSystem.TryDrain(Time.deltaTime))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

            if (moveX != 0)
                transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);
            
        animator.SetBool("isWalking", Mathf.Abs(moveX) > 0.1f);

        if (jumpAction.triggered && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }
        
        //fall = death

        if (rb.linearVelocity.y < -0.1f)
        {
            fallTimer += Time.deltaTime;
            if (!isDead && fallTimer >= maxFallTimer)
            {
                isDead = true;
                GetComponent<HealthSystem>()?.TakeDamage(9999);
            }
        }

        float fallRatio = fallTimer / maxFallTimer;
        if (fallRatio > 0.5f)
        {
            sr.color = Color.Lerp(Color.white, Color.red, (fallRatio - 0.5f) / 0.5f);
        }
        else if (fallTimer > 0.4f)
        {
            sr.color = Color.white;
        }

        // 1 za melee
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log("nož");

            meleeScript.enabled = true;
            rangedScript.enabled = false;
            playerExtend.enabled = false;
            
            extendWeaponObject.SetActive(false);
            rifleObject.SetActive(false);

            animator.SetBool("hasRifle", false);
            
            weaponHUD?.SetActiveSlot(0);
        }
        // 2 za ranged
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("puška");

            meleeScript.enabled = false;
            rangedScript.enabled = true;
            playerExtend.enabled = false;
            
            extendWeaponObject.SetActive(false);
            rifleObject.SetActive(true);

            animator.SetBool("hasRifle", true);
            
            weaponHUD?.SetActiveSlot(1);
        }

        // 3 za extend weapon
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Debug.Log("pike");

            meleeScript.enabled = false;
            rangedScript.enabled = false;
            playerExtend.enabled = true;

            rifleObject.SetActive(false);
            extendWeaponObject.SetActive(true);

            animator.SetBool("hasRifle", true);
            
            weaponHUD?.SetActiveSlot(2);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        fallTimer = 0;
        jumpCount = 0;
    }
}
}
