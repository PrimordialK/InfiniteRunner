using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10;

    private Collider2D hitboxCollider;
    private Animator enemyAnimator;

    void Start()
    {
        hitboxCollider = GetComponent<Collider2D>();
        hitboxCollider.enabled = false;

        enemyAnimator = GetComponentInParent<Animator>();
        if (enemyAnimator == null)
            Debug.LogError("EnemyAttackHitbox could not find an Animator on the parent. Make sure it is a child of the Enemy.");
    }

    // Called by Animation Event at the frame the attack should start hitting
    public void EnableHitbox() => hitboxCollider.enabled = true;

    // Called by Animation Event at the frame the attack should stop hitting
    public void DisableHitbox() => hitboxCollider.enabled = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent(out PlayerController player))
        {
            player.TakeDamage(attackDamage);
            Debug.Log($"{transform.parent.name} hit player for {attackDamage} damage.");
        }
    }
}