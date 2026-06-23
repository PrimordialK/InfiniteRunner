using UnityEngine;

public class EnemyAnimationEventRelay : MonoBehaviour
{
    private EnemyAttackHitbox attackHitbox;

    void Start()
    {
        attackHitbox = GetComponentInChildren<EnemyAttackHitbox>();
        if (attackHitbox == null)
            Debug.LogError("EnemyAnimationEventRelay could not find an EnemyAttackHitbox in children.");
    }

    // Called by Animation Event
    public void EnableHitbox() => attackHitbox?.EnableHitbox();

    // Called by Animation Event
    public void DisableHitbox() => attackHitbox?.DisableHitbox();
}