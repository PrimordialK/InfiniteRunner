using UnityEngine;


public class HealthBarUI : MonoBehaviour
{

    public float MaxHealth, Health, Height, Width;


    [SerializeField] private RectTransform healthBar;




    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;

    }

    public void SetHealth(float health)
    {
        Health = health;
        float newWidth = (Health / MaxHealth) * Width;

        healthBar.sizeDelta = new Vector2(newWidth, Height);
    }





}
