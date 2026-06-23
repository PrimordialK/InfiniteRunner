using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private int attack1Damage = 10;
    [SerializeField] private int attack2Damage = 20;

    private Collider2D hitboxCollider;
    private Animator playerAnimator;
    private SpriteRenderer parentSpriteRenderer;
    private bool _hasHitThisSwing;
    private Vector3 _originalLocalPosition;

    void Start()
    {
        hitboxCollider = GetComponent<Collider2D>();
        hitboxCollider.enabled = false;

        playerAnimator = GetComponentInParent<Animator>();
        if (playerAnimator == null)
            Debug.LogError("AttackHitbox could not find an Animator on the parent. Make sure it is a child of the Player.");

        parentSpriteRenderer = GetComponentInParent<SpriteRenderer>();
        if (parentSpriteRenderer == null)
            Debug.LogError("AttackHitbox could not find a SpriteRenderer on the parent.");

        // Store the default local position set in the editor
        _originalLocalPosition = transform.localPosition;
    }

    void Update()
    {
        AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        bool isAttacking = currentState.IsName("Player_Attack_1") || currentState.IsName("Player_Attack_2");

        // Reset hit flag each time a new swing starts
        if (isAttacking && !hitboxCollider.enabled)
            _hasHitThisSwing = false;

        hitboxCollider.enabled = isAttacking;

        // Mirror the hitbox X position to match the sprite flip direction
        float xOffset = parentSpriteRenderer.flipX ? -_originalLocalPosition.x : _originalLocalPosition.x;
        transform.localPosition = new Vector3(xOffset, _originalLocalPosition.y, _originalLocalPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy") || _hasHitThisSwing) return;

        AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        int damage = currentState.IsName("Player_Attack_2") ? attack2Damage : attack1Damage;

        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            _hasHitThisSwing = true;
            Debug.Log($"Player hit {other.gameObject.name} for {damage} damage.");
        }
    }
}