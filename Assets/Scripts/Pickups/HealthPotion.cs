using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent(out PlayerController player))
        {
            player.SetHealth(player.MaxHealth - player.Health);
            Debug.Log("Health potion collected. Player health restored to max.");
            Destroy(gameObject);
        }
    }
}
