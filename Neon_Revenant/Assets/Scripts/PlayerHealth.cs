using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Image healthBarFill; // Drag the green bar image here (Image Type: Filled)

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Update()
    {
        // Press H to take damage
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }

        // Press J to heal
        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(10);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        fillAmount = Mathf.Max(fillAmount, 0.08f);
        healthBarFill.fillAmount = fillAmount;
    }

    void Die()
    {
        Debug.Log("Player Died");
        // Add death logic here
    }
    
}
