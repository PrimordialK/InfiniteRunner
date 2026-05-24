using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public const int freezeDamage = 10;
    protected Rigidbody2D rb;

    protected Collider2D col;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected int health;
    [SerializeField] private int maxHealth = 20;
    [SerializeField] public int knockBackForce = 5;

    public AudioClip deathSound;
    private AudioSource audioSource;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>(); // ← was missing

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

    public virtual void TakeDamage(int damageValue, DamageType damageType = DamageType.Projectile)
    {
        if (!enabled) return; // Ignore damage if already dead

        health -= damageValue;

        if (health <= 0)
        {
            animator.SetTrigger("Dead");
            audioSource?.PlayOneShot(deathSound);
            spriteRenderer.enabled = false;
            col.enabled = false;
            enabled = false; // Disable this component to prevent further damage
            Destroy(gameObject, 5.0f);
        }
        else
        {
            animator.SetTrigger("Hurt");
            Vector2 knockbackDir = (transform.position - FindObjectOfType<PlayerController>().transform.position).normalized;
            rb.AddForce(knockbackDir * knockBackForce, ForceMode2D.Impulse);
        }
    }

    public enum DamageType
    {
        Projectile,
        Freeze
    }
}


