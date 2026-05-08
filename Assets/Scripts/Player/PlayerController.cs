using UnityEngine;
using UnityEngine.Audio;


[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region 
    #region SerializedFields
    [SerializeField] private float groundCheckRadius = 0.02f;
    #endregion

    #region AudioClips
    private AudioClip jumpSound;
    private AudioClip currentJumpSound;
    #endregion

    #region Components
    private AudioSource audioSource;
    private LayerMask groundLayer;
    public Transform playerTransform;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D collider;
    private GroundCheck groundCheck;
    #endregion

    #region Variables
    private float initialGroundCheckRadius;

    #endregion
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        groundCheck = new GroundCheck(collider, groundLayer, initialGroundCheckRadius);

        groundLayer = LayerMask.GetMask("Ground");

        if (groundLayer == 0)
        {
            Debug.LogError("Ground layer not set. Please set the Ground layer in the LayerMask.");
        }
        groundCheck = new GroundCheck(collider, groundLayer, groundCheckRadius);
        initialGroundCheckRadius = groundCheckRadius;

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
        
    }
}
