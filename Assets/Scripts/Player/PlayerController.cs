using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private float groundCheckRadius = 0.02f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GestureDetector gestureDetector;

    [Header("Health")]
    [SerializeField] private HealthBarUI healthBar;
    public float Health, MaxHealth;

    #endregion

    #region SerializedAudioClips
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip currentJumpSound;
    [SerializeField] private AudioClip attackSound1;
    [SerializeField] private AudioClip attackSound2;
    #endregion

    #region Components
    private AudioSource audioSource;
    public Transform playerTransform;
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Collider2D col;
    public GroundCheck groundCheck;
    #endregion

    #region Variables
    private float initialGroundCheckRadius;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float moveSpeed = 5f;
    private float soundDelay = 1.0f;


    #endregion

    #region Combat
    [Header("Combat Settings")]
    [Tooltip("Time window after first attack to register a combo tap (seconds)")]
    [SerializeField] private float comboWindow = 2f;
    private bool comboQueued = false;
    private float lastAttackTime = -999f;
    #endregion

    #region Health 
    public void SetHealth(float healthChange) // Applies a health change (positive = heal, negative = damage)
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.SetHealth(Health);
    }

    public void TakeDamage(int amount) // Reduces health by amount, triggers death at zero
    {
        SetHealth(-amount);
        Debug.Log($"Player took {amount} damage. Health: {Health}/{MaxHealth}");

        if (Health <= 0)
            Die();
    }

    private void Die() // Loads the Game Over scene on death
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene(2);
    }
    #endregion

    [SerializeField] private InputActionReference moveAction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        Health = MaxHealth;

        if (groundLayer == 0)
            Debug.LogError("Ground layer not set. Please set the Ground layer in the LayerMask.");

        initialGroundCheckRadius = groundCheckRadius;
        groundCheck = new GroundCheck(col, groundLayer, groundCheckRadius);

        if (jumpSound != null)
        {
            TryGetComponent(out audioSource);
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                Debug.Log("AudioSource component was missing. Added one dynamically.");
            }
        }
        currentJumpSound = jumpSound;

        if (gestureDetector != null)
        {
            gestureDetector.OnTap += HandleTap;
            gestureDetector.OnSwipe += HandleSwipe;
        }
        else
        {
            Debug.LogWarning("GestureDetector not assigned in PlayerController.");
        }
    }

    void OnDisable()
    {
        if (gestureDetector != null)
        {
            gestureDetector.OnTap -= HandleTap;
            gestureDetector.OnSwipe -= HandleSwipe;
        }
    }

    private void HandleTap()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (currentState.IsName("Player_Attack_1"))
        {
            // Player tapped again during Attack1 — queue the combo
            comboQueued = true;
            audioSource.clip = attackSound1;
            audioSource.Play();
        }
        else if (!currentState.IsName("Player_Attack_2"))
        {
            // No attack playing — start Attack1
            animator.SetTrigger("Attack1");
            lastAttackTime = Time.time;
            comboQueued = false;
            audioSource.Stop();
            audioSource?.PlayOneShot(attackSound2);
        }
    }

    private void HandleSwipe(SwipeDirection direction)
    {
        if (direction == SwipeDirection.Up && groundCheck.IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            audioSource?.PlayOneShot(currentJumpSound);
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (!other.CompareTag("Enemy")) return;

    //    AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
    //    bool isAttacking = currentState.IsName("Player_Attack_1") || currentState.IsName("Player_Attack_2");

    //    if (!isAttacking) return;

    //    if (other.TryGetComponent(out Enemy enemy))
    //        enemy.TakeDamage(attackDamage);
    //}

    // Update is called once per frame
    void Update()
    {

        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
        float vValue = moveDirection.y;
        float hValue = moveDirection.x;
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // Freeze movement during attacks
        if (currentState.IsName("Player_Attack_1") || currentState.IsName("Player_Attack_2"))
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            transform.Translate(new Vector2(moveDirection.x, 0f) * moveSpeed * Time.deltaTime);
        }

        // When Attack1 finishes, check if a combo was queued and still within the window
        if (!currentState.IsName("Player_Attack_1") && !currentState.IsName("Player_Attack_2"))
        {
            if (comboQueued && Time.time - lastAttackTime <= comboWindow)
            {
                animator.SetTrigger("Attack2");
                comboQueued = false;
            }
            else if (Time.time - lastAttackTime > comboWindow)
            {
                // Window expired, clear the queue
                comboQueued = false;
            }
        }
      


        SpriteFlip(hValue);
        groundCheck.CheckIsGrounded();

        animator.SetFloat("hValue", Mathf.Abs(hValue));
        animator.SetBool("isGrounded", groundCheck.IsGrounded);
        animator.SetFloat("vValue", Mathf.Abs(vValue));

        if (initialGroundCheckRadius != groundCheckRadius)
            groundCheck.UpdateGroundCheckRadius(groundCheckRadius);
    }

    void SpriteFlip(float hValue)
    {
        if (hValue != 0) spriteRenderer.flipX = (hValue < 0);
    }
}
