using UnityEngine;
using UnityEngine.Audio;

public class TheButcher : Enemy
{
    private Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        // Find the player by tag (make sure your player is tagged "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

       

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null)
            return;
    }
}
