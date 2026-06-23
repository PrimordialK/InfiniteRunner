using UnityEngine;

public class TheSkeleton : Enemy
{
    private Transform playerTransform;

    protected override void Start()
    {
        base.Start();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogWarning("TheSkeleton: No GameObject found with tag 'Player'.");
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Face the player — hitbox flips automatically as a child
        float direction = playerTransform.position.x - transform.position.x;
        FaceDirection(direction);
        ChasePlayer(playerTransform);
    }
}