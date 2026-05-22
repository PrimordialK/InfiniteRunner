using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private float groundCheckRadius = 0.02f;
    [SerializeField] private LayerMask groundLayer;
    #endregion

    #region AudioClips
    private AudioClip jumpSound;
    private AudioClip currentJumpSound;
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
    private float jumpForce = 6f;

    [SerializeField] private float moveSpeed = 5f;
    #endregion

    [SerializeField] private InputActionReference moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        //groundLayer = LayerMask.GetMask("Ground");

        if (groundLayer == 0)
        {
            Debug.LogError("Ground layer not set. Please set the Ground layer in the LayerMask.");
        }

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
    }


    // Update is called once per frame
    void Update()
    {

        Vector2 moveDirection = moveAction.action.ReadValue<Vector2>();
        transform.Translate(new Vector2 (moveDirection.x, 0f) * moveSpeed * Time.deltaTime);

        float vValue = moveDirection.y;
        float hValue = moveDirection.x;
        animator.SetFloat("hValue", Mathf.Abs(hValue));
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        SpriteFlip(hValue);

        // Hold S to crouch, release S to stand


        //bool isCrouching = Input.GetKey(KeyCode.S) && groundCheck.IsGrounded;


        //float moveSpeed = isCrouching ? 0f : 5f;
        //rb.linearVelocityX = hValue * moveSpeed;


        groundCheck.CheckIsGrounded();

        //if (!currentState.IsName("Fire") && Input.GetButtonDown("Fire1"))
        //{
        //    animator.SetTrigger("Fire");
        //}
        //else if (currentState.IsName("Fire"))
        //{
        //    rb.linearVelocity = Vector2.zero;
        //}


        //if (Input.GetButtonDown("Jump") && groundCheck.IsGrounded)
        //{
        //    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //    audioSource?.PlayOneShot(currentJumpSound);
        //}

        //// Set crouch/stand triggers based on S key
        //if (isCrouching && !currentState.IsName("Crouch"))
        //{
        //    animator.SetTrigger("Crouch");
        //}
        //else if (!isCrouching && currentState.IsName("Crouch"))
        //{
        //    animator.SetTrigger("Standing");
        //}

        // Update the animator parameters
        animator.SetFloat("hValue", Mathf.Abs(hValue));
        animator.SetBool("isGrounded", groundCheck.IsGrounded);
        animator.SetFloat("vValue", Mathf.Abs(vValue));
        //animator.SetBool("isCrouching", isCrouching);

        if (initialGroundCheckRadius != groundCheckRadius)
            groundCheck.UpdateGroundCheckRadius(groundCheckRadius);
    }

    void SpriteFlip(float hValue)
    {
        if (hValue != 0) spriteRenderer.flipX = (hValue < 0);
    }
}
