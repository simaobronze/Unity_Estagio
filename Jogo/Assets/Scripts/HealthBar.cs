using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public float Health, MaxHealth, Width, Height;

    [SerializeField]
    private RectTransform _healthBar;

    public void SetMaxHealth (float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void SetHealth(float health)
    {
        Health = health;
        float newWidth = Width * (Health / MaxHealth);

        _healthBar.sizeDelta = new Vector2(newWidth, Height);
    }
}
