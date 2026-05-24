using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private Collider2D hitboxCollider;
    private Animator playerAnimator;

    void Start()
    {
        hitboxCollider = GetComponent<Collider2D>();
        hitboxCollider.enabled = false;

        playerAnimator = GetComponentInParent<Animator>();
        if (playerAnimator == null)
            Debug.LogError("AttackHitbox could not find an Animator on the parent. Make sure it is a child of the Player.");
    }

    void Update()
    {
        AnimatorStateInfo currentState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        bool isAttacking = currentState.IsName("Player_Attack_1") || currentState.IsName("Player_Attack_2");

        // Enable the hitbox collider only while attack animations are playing
        hitboxCollider.enabled = isAttacking;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        Debug.Log($"Destroying enemy: {other.gameObject.name}");
        Destroy(other.gameObject);
    }
}