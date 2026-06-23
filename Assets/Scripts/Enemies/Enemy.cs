using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected int health;
    [SerializeField] private int maxHealth = 20;
    [SerializeField] public int knockBackForce = 5;
    [SerializeField] public float timeBeforeDestroy = 5.0f;
    [Tooltip("Uncheck this if your enemy sprite faces left by default")]
    [SerializeField] private bool spriteFacesRight = true;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float chaseRange = 5f;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float attackCooldown = 1.5f;

    private float _nextAttackTime;

    public AudioClip deathSound;
    private AudioSource audioSource;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (maxHealth <= 0)
        {
            Debug.Log("maxHealth must be greater than 0. Setting to 20.");
            maxHealth = 20;
        }
        health = maxHealth;

        if (deathSound != null)
        {
            TryGetComponent(out audioSource);
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    protected void FaceDirection(float horizontalDirection)
    {
        if (horizontalDirection == 0) return;

        bool shouldFaceRight = horizontalDirection > 0;
        bool isFacingRight = spriteFacesRight ? transform.localScale.x > 0 : transform.localScale.x < 0;

        if (shouldFaceRight == isFacingRight) return; // Already facing correct direction

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected void ChasePlayer(Transform playerTransform)
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= attackRange)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            animator.SetBool("Walk", false);

            if (Time.time >= _nextAttackTime)
            {
                animator.SetTrigger("Attack");
                _nextAttackTime = Time.time + attackCooldown;
            }
        }
        else if (distance <= chaseRange)
        {
            float direction = playerTransform.position.x - transform.position.x;
            animator.SetBool("Walk", true);
            rb.velocity = new Vector2(Mathf.Sign(direction) * moveSpeed, rb.velocity.y);
        }
        else
        {
            animator.SetBool("Walk", false);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    // Override in subclasses to run logic at the moment of death, before the component is disabled
    protected virtual void OnDeath() { }

    public virtual void TakeDamage(int damageValue)
    {
        if (!enabled) return;

        health -= damageValue;

        if (health <= 0)
        {
            animator.SetTrigger("Dead");
            animator.SetBool("Walk", false);
            OnDeath(); // Called before enabled = false so coroutines can still be started
            enabled = false;
            audioSource?.PlayOneShot(deathSound);
            Destroy(gameObject, timeBeforeDestroy);
        }
        else
        {
            animator.SetTrigger("Hurt");
            Vector2 knockbackDir = (transform.position - FindObjectOfType<PlayerController>().transform.position).normalized;
            rb.AddForce(knockbackDir * knockBackForce, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}


