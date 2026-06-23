using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private GameObject blueAuraPrefab; // Assign your aura prefab in the Inspector

    private Transform playerTransform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.transform; // Store reference to the player's Transform
            Destroy(gameObject); // Remove the power-up after collection
        }
    }

    private void OnDestroy()
    {
        if (CompareTag("PowerUps"))
        {
            // Instantiate the blue aura effect at the player's position and play its animation
            if (blueAuraPrefab != null && playerTransform != null)
            {
                GameObject aura = Instantiate(blueAuraPrefab, playerTransform.position, Quaternion.identity);
                Animator animator = aura.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("BlueAuraStart01");
                }
            }
        }
    }
}


